using EventBooking.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Infrastructure.Unitofwork
{
	public interface IUnitOfWork : IDisposable
	{
		IEventRepository Events { get; }
		IBookingRepository Bookings { get; }
		Task<int> CompleteAsync();
	}
}
