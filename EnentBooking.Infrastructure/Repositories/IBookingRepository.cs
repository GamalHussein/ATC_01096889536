using EventBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Infrastructure.Repositories
{
	public interface IBookingRepository : IRepository<Booking>
	{
		Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);
		Task<bool> HasUserBookedEventAsync(string userId, int eventId);
	}

}
