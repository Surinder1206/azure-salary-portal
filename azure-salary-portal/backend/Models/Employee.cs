using Azure;
using Azure.Data.Tables;
using System.Text.Json.Serialization;

namespace PayslipPortal.Functions.Models;

public class Employee : ITableEntity
{
    public string PartitionKey { get; set; } = "Employee";
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    [JsonPropertyName("employeeId")]
    public string EmployeeId
    {
        get => RowKey;
        set => RowKey = value;
    }

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("roles")]
    public string RolesJson { get; set; } = "[]";

    [JsonIgnore]
    public List<string> Roles
    {
        get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(RolesJson) ?? new List<string>();
        set => RolesJson = System.Text.Json.JsonSerializer.Serialize(value);
    }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = true;

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("lastModified")]
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}