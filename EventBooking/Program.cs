using EventBooking.Infrastructure;
using EventBooking.Application;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using EventBooking.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EventBooking.Api.HostService;
using EventBooking.Api.Mapping;
using Mapster;

namespace EventBooking
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt=>
			{
				opt.User.RequireUniqueEmail = true;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				
				.AddDefaultTokenProviders();


			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyOrigin()      // ?? ?????? .WithOrigins("https://example.com") ?????? ????? ?????
						  .AllowAnyMethod()
						  .AllowAnyHeader();
				});
			});


			builder.Services.AddMapster(); // registers IMapper
			TypeAdapterConfig.GlobalSettings.Scan(typeof(MappingConfig).Assembly);



			builder.Services.AddInfrastructure(builder.Configuration);
			builder.Services.AddApplication();
			builder.Services.ConfigureApiVersioning();
			builder.Services.ConfigureSwagger();

			var jwtKey = Environment.GetEnvironmentVariable("EVENT_BOOKING_SECRET"); // ???? null
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["JwtSettings:issuer"],
					ValidAudience = builder.Configuration["JwtSettings:audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("alsdasdasdas455sdfsdfsd#sdfsdf5564sdfsdfsdfsllmsmdfsdfnmnsdfsdfsdscxcsdlsdfsdf")),
					
					
				};
			});


			builder.Services.AddHostedService<SeedDataHostedService>();

			

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventBooking API v1"));
			}
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
				c.RoutePrefix = string.Empty; // ?? ??? ????? ???? swagger ?? ?????? ????????
			});


			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthorization();

		
			app.MapControllers();
			
			app.Run();
        }
    }
}
