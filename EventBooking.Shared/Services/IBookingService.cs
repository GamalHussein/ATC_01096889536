using EventBooking.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Services
{
	public interface IBookingService
	{
		Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId);
		Task<BookingDto> GetBookingByIdAsync(int id);
		Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto);
		Task<bool> HasUserBookedEventAsync(string userId, int eventId);
		Task<bool> DeleteBookingAsync(int id, string userId);
	}
}
