using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Data;
using EventBooking.Infrastructure.Repositories;
using EventBooking.Infrastructure.Unitofwork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBooking.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			// Register DbContext
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					configuration.GetConnectionString("dbConnection"),
					b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));	

			// Register Repositories
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<IEventRepository, EventRepository>();
			services.AddScoped<IBookingRepository, BookingRepository>();

			// Register UnitOfWork
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}

		public static async Task SeedDataAsync(IServiceProvider serviceProvider,IConfiguration configuration)
		{
			using var scope = serviceProvider.CreateScope();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			// Seed Roles
			await SeedRolesAsync(roleManager);

			// Seed Admin User
			await SeedAdminUserAsync(userManager, configuration);
		}

		private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync(Roles.Admin))
			{
				await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
			}

			if (!await roleManager.RoleExistsAsync(Roles.User))
			{
				await roleManager.CreateAsync(new IdentityRole(Roles.User));
			}
		}

		private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
		{
			var adminEmail = configuration["Admin:email"];
			var admin = await userManager.FindByEmailAsync(adminEmail);

			if (admin == null)
			{
				admin = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					FirstName = "Admin",
					LastName = "User",
					EmailConfirmed = true
				};

				await userManager.CreateAsync(admin, configuration["Admin:password"]); // Strong password for demo purposes
				await userManager.AddToRoleAsync(admin, Roles.Admin);
			}
		}
	}
}
