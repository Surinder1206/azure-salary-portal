using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PayslipPortal.Functions.Models;
using System.Net;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Data.Tables;

namespace PayslipPortal.Functions.Functions;

public class PayslipFunctions
{
    private readonly ILogger _logger;
    private readonly TableClient _payslipTableClient;
    private readonly BlobServiceClient _blobServiceClient;

    public PayslipFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PayslipFunctions>();

        // Initialize Azure clients
        var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString")
            ?? "UseDevelopmentStorage=true";

        _payslipTableClient = new TableClient(connectionString, "payslips");
        _blobServiceClient = new BlobServiceClient(connectionString);

        // Ensure table exists
        _payslipTableClient.CreateIfNotExists();
    }

    [Function("GetPayslips")]
    public async Task<HttpResponseData> GetPayslips(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "payslips")] HttpRequestData req)
    {
        _logger.LogInformation("GetPayslips function processed a request.");

        try
        {
            // Get user ID from SWA headers
            var userId = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ID").FirstOrDefault();

            if (string.IsNullOrEmpty(userId))
            {
                var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("User not authenticated");
                return unauthorizedResponse;
            }

            // Query payslips for the user
            var payslips = new List<object>();

            await foreach (var payslip in _payslipTableClient.QueryAsync<Payslip>(p => p.PartitionKey == userId))
            {
                payslips.Add(new
                {
                    Id = payslip.PayslipId,
                    TaxYear = payslip.TaxYear,
                    Period = payslip.Period,
                    GrossPay = payslip.GrossPay,
                    NetPay = payslip.NetPay,
                    PayDate = payslip.PayDate,
                    FileName = payslip.FileName
                });
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(payslips));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPayslips function");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("GetPayslipDownload")]
    public async Task<HttpResponseData> GetPayslipDownload(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "payslips/{id}/download")] HttpRequestData req,
        string id)
    {
        _logger.LogInformation($"GetPayslipDownload function processed a request for ID: {id}");

        try
        {
            // Get user ID from SWA headers
            var userId = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ID").FirstOrDefault();

            if (string.IsNullOrEmpty(userId))
            {
                var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("User not authenticated");
                return unauthorizedResponse;
            }

            // Get payslip from table
            var payslipResponse = await _payslipTableClient.GetEntityAsync<Payslip>(userId, id);
            var payslip = payslipResponse.Value;

            if (payslip == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("Payslip not found");
                return notFoundResponse;
            }

            // Generate SAS URL for blob
            var containerName = Environment.GetEnvironmentVariable("StorageBlobContainerName") ?? "payslips";
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(payslip.BlobPath);

            // Generate SAS token (valid for 1 hour)
            var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read,
                DateTimeOffset.UtcNow.AddHours(1));

            var downloadInfo = new
            {
                DownloadUrl = sasUri.ToString(),
                FileName = payslip.FileName,
                ContentType = payslip.ContentType,
                FileSize = payslip.FileSize,
                ExpiresAt = DateTimeOffset.UtcNow.AddHours(1)
            };

            // Log audit entry
            var auditLog = AuditLog.Create(
                userId,
                "DOWNLOAD",
                "Payslip",
                id,
                $"Downloaded payslip: {payslip.FileName}"
            );

            var auditTableClient = new TableClient(_payslipTableClient.Uri.ToString().Replace("/payslips", "/auditlogs"),
                "auditlogs");
            await auditTableClient.CreateIfNotExistsAsync();
            await auditTableClient.UpsertEntityAsync(auditLog);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(downloadInfo));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetPayslipDownload function for ID: {id}");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }
}