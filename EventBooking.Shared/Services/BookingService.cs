using EventBooking.Application.Dtos;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Unitofwork;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Services
{
	public class BookingService : IBookingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId)
		{
			var bookings = await _unitOfWork.Bookings.GetUserBookingsAsync(userId);

			var result=_mapper.Map<IEnumerable<BookingDto>>(bookings);
			return result;
			
		}

		public async Task<BookingDto> GetBookingByIdAsync(int id)
		{
			var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
			if (booking == null)
				return null;

			var @event = await _unitOfWork.Events.GetByIdAsync(booking.EventId);
			var result = _mapper.Map<BookingDto>(booking);
			return result;
			
		}

		public async Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto)
		{
			var @event = await _unitOfWork.Events.GetByIdAsync(createBookingDto.EventId);
			if (@event == null)
				return null;

			var booking = new Booking
			{
				UserId = userId,
				EventId = createBookingDto.EventId,
				BookingDate = DateTime.UtcNow,
				TicketCount = createBookingDto.TicketCount,
				IsConfirmed = true // Auto-confirm for now
			};

			await _unitOfWork.Bookings.AddAsync(booking);
			await _unitOfWork.CompleteAsync();

			var result = _mapper.Map<BookingDto>(booking);
			return result;

			
		}

		public async Task<bool> HasUserBookedEventAsync(string userId, int eventId)
		{
			return await _unitOfWork.Bookings.HasUserBookedEventAsync(userId, eventId);
		}

		public async Task<bool> DeleteBookingAsync(int id, string userId)
		{
			var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
			if (booking == null || booking.UserId != userId)
				return false;

			_unitOfWork.Bookings.Delete(booking);
			await _unitOfWork.CompleteAsync();
			return true;
		}
	}
}
