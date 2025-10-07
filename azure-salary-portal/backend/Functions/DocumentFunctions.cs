using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PayslipPortal.Functions.Models;
using System.Net;
using System.Text.Json;
using Azure.Data.Tables;

namespace PayslipPortal.Functions.Functions;

public class DocumentFunctions
{
    private readonly ILogger _logger;
    private readonly TableClient _documentTableClient;

    public DocumentFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DocumentFunctions>();

        // Initialize Azure clients
        var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString")
            ?? "UseDevelopmentStorage=true";

        _documentTableClient = new TableClient(connectionString, "documents");

        // Ensure table exists
        _documentTableClient.CreateIfNotExists();
    }

    [Function("GetDocuments")]
    public async Task<HttpResponseData> GetDocuments(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "docs")] HttpRequestData req)
    {
        _logger.LogInformation("GetDocuments function processed a request.");

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

            // Query documents for the user
            var documents = new List<object>();

            await foreach (var doc in _documentTableClient.QueryAsync<Document>(d => d.PartitionKey == userId))
            {
                documents.Add(new
                {
                    Id = doc.DocumentId,
                    Name = doc.Name,
                    Type = doc.DocumentType,
                    Description = doc.Description,
                    TaxYear = doc.TaxYear,
                    CreatedDate = doc.CreatedDate,
                    FileSize = doc.FileSize,
                    FileName = doc.FileName
                });
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(documents));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetDocuments function");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("GetDocumentDownload")]
    public async Task<HttpResponseData> GetDocumentDownload(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "docs/{id}/download")] HttpRequestData req,
        string id)
    {
        _logger.LogInformation($"GetDocumentDownload function processed a request for ID: {id}");

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

            // Get document from table
            var documentResponse = await _documentTableClient.GetEntityAsync<Document>(userId, id);
            var document = documentResponse.Value;

            if (document == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("Document not found");
                return notFoundResponse;
            }

            // Generate SAS URL for blob (similar to payslip download)
            var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString")
                ?? "UseDevelopmentStorage=true";
            var blobServiceClient = new Azure.Storage.Blobs.BlobServiceClient(connectionString);
            var containerName = Environment.GetEnvironmentVariable("StorageBlobContainerName") ?? "documents";
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(document.BlobPath);

            // Generate SAS token (valid for 1 hour)
            var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read,
                DateTimeOffset.UtcNow.AddHours(1));

            var downloadInfo = new
            {
                DownloadUrl = sasUri.ToString(),
                FileName = document.FileName,
                ContentType = document.ContentType,
                FileSize = document.FileSize,
                ExpiresAt = DateTimeOffset.UtcNow.AddHours(1)
            };

            // Log audit entry
            var auditLog = AuditLog.Create(
                userId,
                "DOWNLOAD",
                "Document",
                id,
                $"Downloaded document: {document.FileName}"
            );

            var auditTableClient = new TableClient(connectionString, "auditlogs");
            await auditTableClient.CreateIfNotExistsAsync();
            await auditTableClient.UpsertEntityAsync(auditLog);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(downloadInfo));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetDocumentDownload function for ID: {id}");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }
}