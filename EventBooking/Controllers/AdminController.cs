using EventBooking.Application.Dtos;
using EventBooking.Application.Services;
using EventBooking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBooking.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = Roles.Admin)]
	public class AdminController : ControllerBase
	{
		private readonly IEventService _eventService;

		public AdminController(IEventService eventService)
		{
			_eventService = eventService;
		}

		// GET: api/Admin/Events
		[HttpGet("get-all-events")]
		public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
		{
			var events = await _eventService.GetAllEventsAsync();
			return Ok(events);
		}

		// POST: api/Admin/Events
		[HttpPost("create-event")]
		public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto createEventDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var createdEvent = await _eventService.CreateEventAsync(createEventDto);
			return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
		}

		// GET: api/Admin/Events/5
		[HttpGet("get-event/{id}")]
		public async Task<ActionResult<EventDto>> GetEvent(int id)
		{
			var @event = await _eventService.GetEventByIdAsync(id);
			if (@event == null)
				return NotFound();

			return Ok(@event);
		}

		// PUT: api/Admin/Events/5
		[HttpPut("update-event/{id}")]
		public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto updateEventDto)
		{
			if (id != updateEventDto.Id)
				return BadRequest("Id mismatch");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!await _eventService.EventExistsAsync(id))
				return NotFound();

			var updatedEvent = await _eventService.UpdateEventAsync(updateEventDto);
			return Ok(updatedEvent);
		}

		// DELETE: api/Admin/Events/5
		[HttpDelete("delete-event/{id}")]
		public async Task<IActionResult> DeleteEvent(int id)
		{
			if (!await _eventService.EventExistsAsync(id))
				return NotFound();

			var result = await _eventService.DeleteEventAsync(id);
			if (!result)
				return BadRequest("Failed to delete event");

			return NoContent();
		}
	}
}
