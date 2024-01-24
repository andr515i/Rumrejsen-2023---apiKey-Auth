using Microsoft.AspNetCore.Mvc;
using Rumrejsen_2023.Models;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Rumrejsen_2023.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class SpaceStationController : Controller
	{
		//private void ValidateApiKey(string userApiKey)
		//{

		//}

		private const string _fullPath = "C:\\Users\\andr515i\\Desktop\\skole\\H3\\web api asp net\\Rumrejsen 2023\\Rumrejsen 2023\\galacticRoutes.txt";
		private const string _prefix = "qt.";
		private const int numberOfSecureBytesToGenerate = 32;
		private const int _lengthOfKey = numberOfSecureBytesToGenerate;
		private List<GalacticRoute> _routes;

		[HttpGet("GenerateApiKey")]
		public string GenerateApiKey()
		{
			byte[] bytes = RandomNumberGenerator.GetBytes(numberOfSecureBytesToGenerate);

			string base64String = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_");

			int keyLength = _lengthOfKey - _prefix.Length;

			return _prefix + base64String[..keyLength];
		}


		[HttpGet("GetCosmicRoutes")]
		public ActionResult<List<GalacticRoute>> GetComsmicRoutes()
		{
			try
			{
				// Read the JSON data from the file
				string jsonData = System.IO.File.ReadAllText(_fullPath);

				// Deserialize the JSON data into GalacticRoutesContainer object
				GalacticRouteList routeList = JsonConvert.DeserializeObject<GalacticRouteList>(jsonData);

				// Access the GalacticRoutes property to get the list of GalacticRoute objects
				List<GalacticRoute> galacticRoutes = routeList.GalacticRoutes;

				return galacticRoutes;
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
