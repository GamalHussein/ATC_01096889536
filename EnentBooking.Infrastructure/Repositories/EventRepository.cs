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
	public class EventRepository : Repository<Event>, IEventRepository
	{
		public EventRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Event>> GetEventsWithBookingsAsync()
		{
			return await _context.Events
				.Include(e => e.Bookings)
				.ToListAsync();
		}

		public async Task<Event> GetEventWithBookingsAsync(int id)
		{
			return await _context.Events
				.Include(e => e.Bookings)
				.FirstOrDefaultAsync(e => e.Id == id);
		}
	}
}
