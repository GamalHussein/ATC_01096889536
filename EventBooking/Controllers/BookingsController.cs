using EventBooking.Application.Dtos;
using EventBooking.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventBooking.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingsController : ControllerBase
	{
		private readonly IBookingService _bookingService;

		public BookingsController(IBookingService bookingService)
		{
			_bookingService = bookingService;
		}

		// GET: api/Bookings
		[HttpGet("get-user-bookings")]
		public async Task<ActionResult<IEnumerable<BookingDto>>> GetUserBookings()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var bookings = await _bookingService.GetUserBookingsAsync(userId);
			return Ok(bookings);
		}


		[HttpGet("get-booking{id}")]
		public async Task<ActionResult<BookingDto>> GetBooking(int id)
		{
			var booking = await _bookingService.GetBookingByIdAsync(id);
			if (booking == null)
				return NotFound();

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (booking.UserId != userId)
				return Forbid();

			return Ok(booking);
		}

		[HttpPost("create-booking")]
		public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto createBookingDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Check if user has already booked this event
			var alreadyBooked = await _bookingService.HasUserBookedEventAsync(userId, createBookingDto.EventId);
			if (alreadyBooked)
				return BadRequest("You have already booked this event");

			var createdBooking = await _bookingService.CreateBookingAsync(userId, createBookingDto);
			if (createdBooking == null)
				return BadRequest("Failed to create booking. Event might not exist.");

			return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, createdBooking);
		}
		[HttpDelete("delete-booking/{id}")]
		public async Task<IActionResult> DeleteBooking(int id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await _bookingService.DeleteBookingAsync(id, userId);
			if (!result)
				return NotFound("Booking not found or you don't have permission to delete it");

			return NoContent();
		}

		// GET: api/Bookings/HasBooked/5
		[HttpGet("HasBooked/{eventId}")]
		public async Task<ActionResult<bool>> HasUserBookedEvent(int eventId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var hasBooked = await _bookingService.HasUserBookedEventAsync(userId, eventId);
			return Ok(hasBooked);
		}
	}
}
