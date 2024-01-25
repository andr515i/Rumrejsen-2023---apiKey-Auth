using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Rumrejsen_2023.Authentication
{
    // Implementation of IAuthorizationFilter for API key authentication
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        // Constructor to inject IConfiguration for accessing configuration settings
        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Method called during authorization to validate API key
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the x-api-key header exists in the HTTP request
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                // Return UnauthorizedObjectResult if API key is missing in the request
                context.Result = new UnauthorizedObjectResult("API key missing");
                return;
            }

            // Retrieve API keys from appsettings.json
            string? cadetApiKey = _configuration.GetValue<string>("Authentication:ApiKeys:Cadet");
            string? captainApiKey = _configuration.GetValue<string>("Authentication:ApiKeys:Captain");

            // Check if the user has a valid API key corresponding to those in appsettings.json
            if (!cadetApiKey.Equals(extractedApiKey) && !captainApiKey.Equals(extractedApiKey))
            {
                // Return UnauthorizedObjectResult if the API key is invalid
                context.Result = new UnauthorizedObjectResult("Invalid API key");
                return;
            }
        }
    }
}
