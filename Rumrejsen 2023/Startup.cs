using AspNetCoreRateLimit;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Rumrejsen_2023.Authentication;

public class Startup
{

	// Constructor that injects IConfiguration for accessing configuration settings
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		// Enable loading configuration from appsettings.json
		services.AddOptions();

		// Enable in-memory cache for rate limit counters and IP rules
		services.AddMemoryCache();

		// Load general configuration from appsettings.json
		services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));

		// Load client rules from appsettings.json
		services.Configure<ClientRateLimitPolicies>(Configuration.GetSection("ClientRateLimitPolicies"));
		Console.WriteLine("here");

		// Inject counter and rules stores
		services.AddInMemoryRateLimiting();

		// Configure Swagger/OpenAPI
		services.AddSwaggerGen(C =>
		{
			C.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
			{
				Description = "The API key to access the API",
				Type = SecuritySchemeType.ApiKey,
				Name = "x-api-key",
				In = ParameterLocation.Header,
				Scheme = "ApiKeyScheme"
			});

			var scheme = new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "ApiKey"
				},
				In = ParameterLocation.Header,
			};

			var requirement = new OpenApiSecurityRequirement
			{
				{ scheme, new List<string>() }
			};

			C.AddSecurityRequirement(requirement);
		});

		// Add controllers and API explorer endpoints
		services.AddControllers();
		services.AddEndpointsApiExplorer();

		// Add and configure the ApiKeyAuthFilter
		services.AddScoped<ApiKeyAuthFilter>();

		// Add MVC services with endpoint routing disabled
		services.AddMvc(option => option.EnableEndpointRouting = false);

		// Configure rate limit configuration (resolvers, counter key builders)
		services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		// Use the client rate limiting middleware
		app.UseClientRateLimiting();

		// Configure the HTTP request pipeline for Swagger/OpenAPI
		app.UseSwagger();
		app.UseSwaggerUI();

		// Enable authorization
		app.UseAuthorization();

		// Use the ApiKeyAuthFilter instead of ApiKeyAuthMiddleware
		// app.UseMiddleware<ApiKeyAuthMiddleware>(); 

		// Use MVC with endpoint routing disabled
		app.UseMvc();
	}
}
