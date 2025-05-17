using EventBooking.Infrastructure.Data;
using EventBooking.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Infrastructure.Unitofwork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		public IEventRepository Events { get; }
		public IBookingRepository Bookings { get; }

		public UnitOfWork(ApplicationDbContext context,
						  IEventRepository eventRepository,
						  IBookingRepository bookingRepository)
		{
			_context = context;
			Events = eventRepository;
			Bookings = bookingRepository;
		}

		public async Task<int> CompleteAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
