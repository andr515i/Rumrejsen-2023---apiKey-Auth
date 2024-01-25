/*
 * Model representing a list of Galactic Routes.
 * Used as a wrapper for a collection of GalacticRoute objects.
 */

namespace Rumrejsen_2023.Models
{
	// Model representing a list of Galactic Routes
	public class GalacticRouteList
	{
		// Property to hold a collection of GalacticRoute objects
		public List<GalacticRoute> GalacticRoutes { get; set; }
	}
}
