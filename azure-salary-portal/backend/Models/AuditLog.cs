using Azure;
using Azure.Data.Tables;
using System.Text.Json.Serialization;

namespace PayslipPortal.Functions.Models;

public class AuditLog : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    [JsonPropertyName("logId")]
    public string LogId
    {
        get => RowKey;
        set => RowKey = value;
    }

    [JsonPropertyName("date")]
    public string Date
    {
        get => PartitionKey;
        set => PartitionKey = value;
    }

    [JsonPropertyName("actor")]
    public string Actor { get; set; } = string.Empty;

    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    [JsonPropertyName("entityType")]
    public string EntityType { get; set; } = string.Empty;

    [JsonPropertyName("entityId")]
    public string EntityId { get; set; } = string.Empty;

    [JsonPropertyName("details")]
    public string Details { get; set; } = string.Empty;

    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; } = string.Empty;

    [JsonPropertyName("userAgent")]
    public string UserAgent { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public DateTime LogTimestamp { get; set; } = DateTime.UtcNow;

    public static AuditLog Create(string actor, string action, string entityType, string entityId, string details = "", string ipAddress = "", string userAgent = "")
    {
        var timestamp = DateTime.UtcNow;
        return new AuditLog
        {
            Date = timestamp.ToString("yyyy-MM-dd"),
            LogId = $"{timestamp:yyyy-MM-dd-HH-mm-ss-fff}-{Guid.NewGuid():N}",
            Actor = actor,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            LogTimestamp = timestamp
        };
    }
}