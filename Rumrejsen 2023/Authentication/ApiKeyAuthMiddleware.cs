/*
 * This middleware is no longer actively used in the project.
 * It has been replaced by the ApiKeyAuthFilter class for key authentication.
 * The middleware version is kept in the project for future reference and understanding.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Rumrejsen_2023.Authentication
{
	// Middleware for API key authentication (not actively used)
	public class ApiKeyAuthMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _configuration;

		// Constructor to initialize middleware with the next delegate and IConfiguration
		public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;
			_configuration = configuration;
		}

		// Method to handle the middleware invocation
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				// Check if there is an x-api-key header in the request
				if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
				{
					// Set status code to 412 - Precondition Failed (missing API key)
					context.Response.StatusCode = 412;
					await context.Response.WriteAsync("API key missing");
					return;
				}

				// Retrieve API keys from appsettings.json
				string? apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
				string? captainApiKey = _configuration.GetValue<string>(AuthConstants.ApiKeyCaptainSectionName);

				// Check if the extracted API key is valid
				if (!apiKey.Equals(extractedApiKey) && !captainApiKey.Equals(extractedApiKey))
				{
					// Set status code to 401 - Unauthorized (invalid API key)
					context.Response.StatusCode = 401;
					await context.Response.WriteAsync("Invalid API key");
					return;
				}

				// If the API key is valid, pass the request to the next middleware
				await _next(context);
			}
			catch (Exception)
			{
				// Rethrow any unhandled exceptions
				throw;
			}
		}
	}
}
