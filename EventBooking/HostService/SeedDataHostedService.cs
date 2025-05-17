using EventBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventBooking.Api.HostService
{
	public class SeedDataHostedService : IHostedService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IConfiguration _configuration;

		public SeedDataHostedService(IServiceProvider serviceProvider, IConfiguration configuration)
		{
			_serviceProvider = serviceProvider;
			_configuration = configuration;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			using var scope = _serviceProvider.CreateScope();

			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			// Seed Roles
			if (!await roleManager.RoleExistsAsync(Roles.Admin))
				await roleManager.CreateAsync(new IdentityRole(Roles.Admin));

			if (!await roleManager.RoleExistsAsync(Roles.User))
				await roleManager.CreateAsync(new IdentityRole(Roles.User));

			// Seed Admin User
			var adminEmail = _configuration["Admin:email"];
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

				var result = await userManager.CreateAsync(admin, _configuration["Admin:password"]);
				if (result.Succeeded)
					await userManager.AddToRoleAsync(admin, Roles.Admin);
			}
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
