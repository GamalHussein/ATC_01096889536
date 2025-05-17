using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace EventBooking.Api.Extensions
{
	public static class ServiceExtensions
	{
		public static void ConfigureApiVersioning(this IServiceCollection services)
		{
			services.AddApiVersioning(options =>
			{
				options.ReportApiVersions = true;
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.DefaultApiVersion = new ApiVersion(1, 0);
			});
		}

		public static void ConfigureSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "EventBooking API",
					Version = "v1",
					Description = "API for Event Booking System",
					Contact = new OpenApiContact
					{
						Name = "gmalhhussein@gmail.com",
						Email = "gmalhhussein@example.com"
					}
				});

				// Add JWT Authentication
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header
						},
						Array.Empty<string>()
					}
				});
			});
		}
	}
}
