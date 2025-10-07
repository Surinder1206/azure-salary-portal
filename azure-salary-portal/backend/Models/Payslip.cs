using Azure;
using Azure.Data.Tables;
using System.Text.Json.Serialization;

namespace PayslipPortal.Functions.Models;

public class Payslip : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    [JsonPropertyName("payslipId")]
    public string PayslipId
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

    [JsonPropertyName("taxYear")]
    public string TaxYear { get; set; } = string.Empty;

    [JsonPropertyName("period")]
    public string Period { get; set; } = string.Empty;

    [JsonPropertyName("blobPath")]
    public string BlobPath { get; set; } = string.Empty;

    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("fileSize")]
    public long FileSize { get; set; }

    [JsonPropertyName("contentType")]
    public string ContentType { get; set; } = "application/pdf";

    [JsonPropertyName("grossPay")]
    public decimal GrossPay { get; set; }

    [JsonPropertyName("netPay")]
    public decimal NetPay { get; set; }

    [JsonPropertyName("taxDeducted")]
    public decimal TaxDeducted { get; set; }

    [JsonPropertyName("niDeducted")]
    public decimal NiDeducted { get; set; }

    [JsonPropertyName("metadataJson")]
    public string MetadataJson { get; set; } = "{}";

    [JsonIgnore]
    public Dictionary<string, object> Metadata
    {
        get => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(MetadataJson) ?? new Dictionary<string, object>();
        set => MetadataJson = System.Text.Json.JsonSerializer.Serialize(value);
    }

    [JsonPropertyName("payDate")]
    public DateTime PayDate { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("lastModified")]
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}