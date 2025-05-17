using EventBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Infrastructure.Repositories
{
	public interface IEventRepository : IRepository<Event>
	{
		Task<IEnumerable<Event>> GetEventsWithBookingsAsync();
		Task<Event> GetEventWithBookingsAsync(int id);
	}
}
