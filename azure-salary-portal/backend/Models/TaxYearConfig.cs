using Azure;
using Azure.Data.Tables;
using System.Text.Json.Serialization;

namespace PayslipPortal.Functions.Models;

public class TaxYearConfig : ITableEntity
{
    public string PartitionKey { get; set; } = "TaxYear";
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    [JsonPropertyName("taxYear")]
    public string TaxYear
    {
        get => RowKey;
        set => RowKey = value;
    }

    [JsonPropertyName("payeThreshold")]
    public decimal PayeThreshold { get; set; } = 12570m;

    [JsonPropertyName("niThreshold")]
    public decimal NiThreshold { get; set; } = 12570m;

    [JsonPropertyName("basicRate")]
    public decimal BasicRate { get; set; } = 0.20m;

    [JsonPropertyName("higherRate")]
    public decimal HigherRate { get; set; } = 0.40m;

    [JsonPropertyName("higherRateThreshold")]
    public decimal HigherRateThreshold { get; set; } = 50270m;

    [JsonPropertyName("niRateEmployee")]
    public decimal NiRateEmployee { get; set; } = 0.12m;

    [JsonPropertyName("niRateEmployer")]
    public decimal NiRateEmployer { get; set; } = 0.138m;

    [JsonPropertyName("studentLoanThreshold")]
    public decimal StudentLoanThreshold { get; set; } = 27295m;

    [JsonPropertyName("studentLoanRate")]
    public decimal StudentLoanRate { get; set; } = 0.09m;

    [JsonPropertyName("configJson")]
    public string ConfigJson { get; set; } = "{}";

    [JsonIgnore]
    public Dictionary<string, object> Config
    {
        get => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(ConfigJson) ?? new Dictionary<string, object>();
        set => ConfigJson = System.Text.Json.JsonSerializer.Serialize(value);
    }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("lastModified")]
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}