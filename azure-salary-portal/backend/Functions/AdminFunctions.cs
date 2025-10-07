using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PayslipPortal.Functions.Models;
using System.Net;
using System.Text.Json;
using Azure.Data.Tables;
using Azure.Storage.Blobs;

namespace PayslipPortal.Functions.Functions;

public class AdminFunctions
{
    private readonly ILogger _logger;
    private readonly TableClient _payslipTableClient;
    private readonly TableClient _employeeTableClient;
    private readonly TableClient _taxConfigTableClient;
    private readonly BlobServiceClient _blobServiceClient;

    public AdminFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<AdminFunctions>();

        // Initialize Azure clients
        var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString")
            ?? "UseDevelopmentStorage=true";

        _payslipTableClient = new TableClient(connectionString, "payslips");
        _employeeTableClient = new TableClient(connectionString, "employees");
        _taxConfigTableClient = new TableClient(connectionString, "taxconfigs");
        _blobServiceClient = new BlobServiceClient(connectionString);

        // Ensure tables exist
        _payslipTableClient.CreateIfNotExists();
        _employeeTableClient.CreateIfNotExists();
        _taxConfigTableClient.CreateIfNotExists();
    }

    [Function("BatchUploadPayslips")]
    public async Task<HttpResponseData> BatchUploadPayslips(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "admin/payslips/batch")] HttpRequestData req)
    {
        _logger.LogInformation("BatchUploadPayslips function processed a request.");

        try
        {
            // Check if user is admin
            var userRoles = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ROLES")
                .FirstOrDefault()?.Split(',') ?? Array.Empty<string>();

            if (!userRoles.Contains("admin"))
            {
                var forbiddenResponse = req.CreateResponse(HttpStatusCode.Forbidden);
                await forbiddenResponse.WriteStringAsync("Admin access required");
                return forbiddenResponse;
            }

            // TODO: Process multipart form data for CSV + PDF files
            // This is a placeholder implementation

            var result = new
            {
                Message = "Batch upload functionality - to be implemented",
                Status = "Success",
                ProcessedCount = 0,
                Errors = new List<string>()
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(result));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in BatchUploadPayslips function");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("GetTaxConfig")]
    public async Task<HttpResponseData> GetTaxConfig(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "admin/config/tax-year/{year}")] HttpRequestData req,
        string year)
    {
        _logger.LogInformation($"GetTaxConfig function processed a request for year: {year}");

        try
        {
            // Check if user is admin
            var userRoles = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ROLES")
                .FirstOrDefault()?.Split(',') ?? Array.Empty<string>();

            if (!userRoles.Contains("admin"))
            {
                var forbiddenResponse = req.CreateResponse(HttpStatusCode.Forbidden);
                await forbiddenResponse.WriteStringAsync("Admin access required");
                return forbiddenResponse;
            }

            try
            {
                var configResponse = await _taxConfigTableClient.GetEntityAsync<TaxYearConfig>("TaxYear", year);
                var config = configResponse.Value;

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(JsonSerializer.Serialize(config));
                return response;
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                // Return default config for new tax year
                var defaultConfig = new TaxYearConfig
                {
                    TaxYear = year
                };

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(JsonSerializer.Serialize(defaultConfig));
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetTaxConfig function for year: {year}");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("UpdateTaxConfig")]
    public async Task<HttpResponseData> UpdateTaxConfig(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "admin/config/tax-year/{year}")] HttpRequestData req,
        string year)
    {
        _logger.LogInformation($"UpdateTaxConfig function processed a request for year: {year}");

        try
        {
            // Check if user is admin
            var userRoles = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ROLES")
                .FirstOrDefault()?.Split(',') ?? Array.Empty<string>();

            if (!userRoles.Contains("admin"))
            {
                var forbiddenResponse = req.CreateResponse(HttpStatusCode.Forbidden);
                await forbiddenResponse.WriteStringAsync("Admin access required");
                return forbiddenResponse;
            }

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var config = JsonSerializer.Deserialize<TaxYearConfig>(requestBody);

            if (config == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid configuration data");
                return badRequestResponse;
            }

            config.TaxYear = year;
            config.LastModified = DateTime.UtcNow;

            await _taxConfigTableClient.UpsertEntityAsync(config);

            // Log audit entry
            var userId = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ID").FirstOrDefault() ?? "unknown";
            var auditLog = AuditLog.Create(
                userId,
                "UPDATE",
                "TaxConfig",
                year,
                $"Updated tax configuration for year: {year}"
            );

            var auditTableClient = new TableClient(_taxConfigTableClient.Uri.ToString().Replace("/taxconfigs", "/auditlogs"),
                "auditlogs");
            await auditTableClient.CreateIfNotExistsAsync();
            await auditTableClient.UpsertEntityAsync(auditLog);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(config));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateTaxConfig function for year: {year}");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("PreviewTaxCalculation")]
    public async Task<HttpResponseData> PreviewTaxCalculation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "admin/tax/preview")] HttpRequestData req)
    {
        _logger.LogInformation("PreviewTaxCalculation function processed a request.");

        try
        {
            // Check if user is admin
            var userRoles = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ROLES")
                .FirstOrDefault()?.Split(',') ?? Array.Empty<string>();

            if (!userRoles.Contains("admin"))
            {
                var forbiddenResponse = req.CreateResponse(HttpStatusCode.Forbidden);
                await forbiddenResponse.WriteStringAsync("Admin access required");
                return forbiddenResponse;
            }

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var previewRequest = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody);

            if (previewRequest == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid preview request");
                return badRequestResponse;
            }

            // TODO: Implement actual tax calculation logic
            var preview = new
            {
                GrossSalary = previewRequest.GetValueOrDefault("grossSalary", 30000m),
                TaxDeducted = 3486m, // Placeholder calculation
                NiDeducted = 2088m,  // Placeholder calculation
                NetSalary = 24426m,  // Placeholder calculation
                CalculationBreakdown = new
                {
                    PayeThreshold = 12570m,
                    TaxableAmount = 17430m,
                    BasicRateTax = 3486m,
                    NiAmount = 2088m
                }
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(preview));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PreviewTaxCalculation function");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }
}