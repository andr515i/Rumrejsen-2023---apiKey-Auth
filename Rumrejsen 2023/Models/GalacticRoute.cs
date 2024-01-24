namespace Rumrejsen_2023.Models
{
	public class GalacticRoute
	{
		public string Name { get; set; }
		public string Start { get; set; }

		public string End { get; set; }

		public string[] NavigationPoints { get; set; }

		public string Duration { get; set; }

		public string FuelUsage { get; set; }
		public string Description { get; set; }



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
