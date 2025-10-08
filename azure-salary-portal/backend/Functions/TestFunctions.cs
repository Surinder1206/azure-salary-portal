using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace PayslipPortal.Functions.Functions;

public class TestFunctions
{
    private readonly ILogger _logger;

    public TestFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TestFunctions>();
    }

    [Function("Test")]
    public async Task<HttpResponseData> Test(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequestData req)
    {
        _logger.LogInformation("Test function executed");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain");
        
        await response.WriteStringAsync("Hello from Azure Functions! Backend is working.");
        return response;
    }

    [Function("Ping")]
    public async Task<HttpResponseData> Ping(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")] HttpRequestData req)
    {
        _logger.LogInformation("Ping function executed");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        
        var result = new { message = "pong", timestamp = DateTime.UtcNow };
        await response.WriteAsJsonAsync(result);
        return response;
    }
}