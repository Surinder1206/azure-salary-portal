using Azure;
using Azure.Data.Tables;
using System.Text.Json.Serialization;

namespace PayslipPortal.Functions.Models;

public class Document : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    [JsonPropertyName("documentId")]
    public string DocumentId
    {
        get => RowKey;
        set => RowKey = value;
    }

    [JsonPropertyName("employeeId")]
    public string EmployeeId
    {
        get => PartitionKey;
        set => PartitionKey = value;
    }

    [JsonPropertyName("documentType")]
    public string DocumentType { get; set; } = string.Empty; // P60, P45, Contract, etc.

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("blobPath")]
    public string BlobPath { get; set; } = string.Empty;

    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("fileSize")]
    public long FileSize { get; set; }

    [JsonPropertyName("contentType")]
    public string ContentType { get; set; } = "application/pdf";

    [JsonPropertyName("taxYear")]
    public string TaxYear { get; set; } = string.Empty;

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("lastModified")]
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}