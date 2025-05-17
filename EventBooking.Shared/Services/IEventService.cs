using EventBooking.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Services
{
	public interface IEventService
	{
		Task<IEnumerable<EventDto>> GetAllEventsAsync();
		Task<EventDto> GetEventByIdAsync(int id);
		Task<EventDto> GetEventByNameAsync(string name);
		Task<EventDto> CreateEventAsync(CreateEventDto createEventDto);
		Task<EventDto> UpdateEventAsync(UpdateEventDto updateEventDto);
		Task<bool> DeleteEventAsync(int id);
		Task<bool> EventExistsAsync(int id);
	}
}
