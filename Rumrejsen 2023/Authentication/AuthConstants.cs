/*
 * Constants related to authentication used in the project.
 */

namespace Rumrejsen_2023.Authentication
{
	// Constants related to authentication
	public class AuthConstants
	{
		// Configuration section name for the API key
		public const string ApiKeySectionName = "Authentication:ApiKey";

		// Configuration section name for the Captain's API key
		public const string ApiKeyCaptainSectionName = "Authentication:CaptainApiKey";

		// Header name for the API key in HTTP requests
		public const string ApiKeyHeaderName = "X-Api-Key";
	}
}
