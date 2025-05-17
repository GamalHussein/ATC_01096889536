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
	public class EventsController : ControllerBase
	{
		private readonly IEventService _eventService;

		public EventsController(IEventService eventService)
		{
			_eventService = eventService;
		}

		
		[HttpGet("get-events")]
		public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
		{
			var events = await _eventService.GetAllEventsAsync();
			return Ok(events);
		}

		[HttpGet("get-event/{id}")]
		public async Task<ActionResult<EventDto>> GetEvent(int id)
		{
			var @event = await _eventService.GetEventByIdAsync(id);
			if (@event == null)
				return NotFound();

			return Ok(@event);
		}

		[HttpGet("get-event/{name:alpha}")]
		public async Task<ActionResult<EventDto>> GetEvent(string name)
		{
			var @event = await _eventService.GetEventByNameAsync(name);
			if (@event == null)
				return NotFound();

			return Ok(@event);
		}
		
	}
}
