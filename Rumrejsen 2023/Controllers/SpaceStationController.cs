using Microsoft.AspNetCore.Mvc;
using Rumrejsen_2023.Models;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Rumrejsen_2023.Authentication;

namespace Rumrejsen_2023.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[ServiceFilter(typeof(ApiKeyAuthFilter))]
	public class SpaceStationController : Controller
	{
		private readonly IConfiguration _configuration;

		// Path to the JSON file containing Galactic Routes data
		private const string jsonFilePath = "GalacticRoutes.txt";

		// Custom API key prefix for generated keys
		private const string _prefix = "qt.";

		// Number of secure bytes to generate for the API key
		private const int numberOfSecureBytesToGenerate = 32;

		// Total length of the API key
		private const int _lengthOfKey = numberOfSecureBytesToGenerate;

		// Constructor that injects IConfiguration for accessing configuration settings
		public SpaceStationController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		// Captain-only endpoint to generate a new API key
		[HttpGet("GenerateApiKey")]
		public string GenerateApiKey()
		{
			// Check if the x-api-key header exists in the request
			if (!HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
			{
				// This check is unlikely to be reached since the ApiKeyAuthFilter should handle it.
				return "Missing API key.";
			}

			// Retrieve the captain key from appsettings.json
			string? captainApiKey = _configuration.GetValue<string>("Authentication:ApiKeys:Captain");

			// Check if the user is a captain; if true, no rate limiting is applied
			if (captainApiKey.Equals(extractedApiKey))
			{
				// Using RandomNumberGenerator from cryptography to generate secure random bytes
				byte[] bytes = RandomNumberGenerator.GetBytes(numberOfSecureBytesToGenerate);

				// Convert the bytes to base64 string and replace characters for URL safety
				string base64String = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_");

				// Calculate the desired key length by subtracting the prefix length
				int keyLength = _lengthOfKey - _prefix.Length;

				// Concatenate the prefix and base64 string to form the final API key
				return _prefix + base64String[..keyLength];
			}

			// Unauthorized access for non-captain users
			HttpContext.Response.StatusCode = 401;
			return "Unauthorized access. Only Captains are allowed to generate API keys.";
		}

		// Endpoint to get the list of Galactic Routes
		[HttpGet("GetCosmicRoutes")]
		public ActionResult<List<GalacticRoute>> GetCosmicRoutes()
		{
			try
			{
				// Read the JSON data from the file
				string jsonData = System.IO.File.ReadAllText(jsonFilePath);

				// Deserialize the JSON data into GalacticRouteList object
				GalacticRouteList routeList = JsonConvert.DeserializeObject<GalacticRouteList>(jsonData);

				// Access the GalacticRoutes property to get the list of GalacticRoute objects
				List<GalacticRoute> galacticRoutes = routeList.GalacticRoutes;

				// Return the list of Galactic Routes
				return galacticRoutes;
			}
			catch (Exception ex)
			{
				// Handle exceptions and return a 500 Internal Server Error status
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
