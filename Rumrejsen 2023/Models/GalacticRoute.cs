/*
 * Model representing a Galactic Route with relevant details.
 */

namespace Rumrejsen_2023.Models
{
	// Model representing a Galactic Route
	public class GalacticRoute
	{
		// Properties of the Galactic Route
		public string Name { get; set; }
		public string Start { get; set; }
		public string End { get; set; }
		public string[] NavigationPoints { get; set; }
		public string Duration { get; set; }
		public string FuelUsage { get; set; }
		public string Description { get; set; }

		// Constructor to initialize Galactic Route properties
		public GalacticRoute(string name, string start, string end, string[] navigation, string duration, string fuelUsage, string description)
		{
			Name = name;
			Start = start;
			End = end;
			NavigationPoints = navigation;
			Duration = duration;
			FuelUsage = fuelUsage;
			Description = description;
		}
	}
}
