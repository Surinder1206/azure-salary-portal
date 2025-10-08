using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace PayslipPortal.Functions.Functions;

public class HealthFunctions
{
    private readonly ILogger _logger;

    public HealthFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<HealthFunctions>();
    }

    [Function("Health")]
    public async Task<HttpResponseData> Health(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req)
    {
        _logger.LogInformation("Health check requested");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");

        var healthInfo = new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            service = "Azure Salary Portal API"
        };

        await response.WriteAsJsonAsync(healthInfo);
        return response;
    }

    [Function("Version")]
    public async Task<HttpResponseData> Version(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "version")] HttpRequestData req)
    {
        _logger.LogInformation("Version check requested");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");

        var versionInfo = new
        {
            version = "1.0.0",
            buildDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            environment = "production",
            features = new[]
            {
                "authentication",
                "payslip-management", 
                "document-storage",
                "admin-functions"
            }
        };

        await response.WriteAsJsonAsync(versionInfo);
        return response;
    }
}