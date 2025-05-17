using EventBooking.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddScoped<IEventService, EventService>();
			services.AddScoped<IBookingService, BookingService>();
			services.AddScoped<IAuthService, AuthService>();

			return services;
		}
	}
}
