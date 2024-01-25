namespace Rumrejsen_2023
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Build and run the host
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					// Configure the web host with the Startup class
					webBuilder.UseStartup<Startup>();
				});
	}
}
