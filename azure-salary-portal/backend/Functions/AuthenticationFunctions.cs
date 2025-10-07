using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace PayslipPortal.Functions.Functions;

public class AuthenticationFunctions
{
    private readonly ILogger _logger;

    public AuthenticationFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<AuthenticationFunctions>();
    }

    [Function("GetMe")]
    public async Task<HttpResponseData> GetMe(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "me")] HttpRequestData req)
    {
        _logger.LogInformation("GetMe function processed a request.");

        try
        {
            // Extract user info from SWA headers
            var userId = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ID").FirstOrDefault();
            var userEmail = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-NAME").FirstOrDefault();
            var userRoles = req.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ROLES")
                .FirstOrDefault()?.Split(',') ?? Array.Empty<string>();

            if (string.IsNullOrEmpty(userId))
            {
                var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("User not authenticated");
                return unauthorizedResponse;
            }

            var userInfo = new
            {
                Id = userId,
                Email = userEmail,
                DisplayName = userEmail?.Split('@')[0], // Simple display name from email
                Roles = userRoles,
                IsAdmin = userRoles.Contains("admin"),
                IsAuthenticated = true
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(userInfo));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetMe function");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }
}