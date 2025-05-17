using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Infrastructure.Repositories
{
	public class BookingRepository : Repository<Booking>, IBookingRepository
	{
		public BookingRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
		{
			return await _context.Bookings
				.Include(b => b.Event)
				.Where(b => b.UserId == userId)
				.ToListAsync();
		}

		public async Task<bool> HasUserBookedEventAsync(string userId, int eventId)
		{
			return await _context.Bookings
				.AnyAsync(b => b.UserId == userId && b.EventId == eventId);
		}
	}
}
