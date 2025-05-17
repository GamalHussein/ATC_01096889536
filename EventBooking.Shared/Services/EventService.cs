using EventBooking.Application.Dtos;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Unitofwork;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EventBooking.Application.Services
{
	public class EventService : IEventService
	{
		private readonly IUnitOfWork _unitOfWork;

		public IMapper _mapper { get; }

		public EventService(IUnitOfWork unitOfWork , IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
		{
			var events = await _unitOfWork.Events.GetAllAsync();

			var result = _mapper.Map<IEnumerable<EventDto>>(events);
			return result;
			
		}

		public async Task<EventDto> GetEventByIdAsync(int id)
		{
			var @event = await _unitOfWork.Events.GetByIdAsync(id);
			if (@event == null)
				return null;

			var result = _mapper.Map<EventDto>(@event);
			return result;
			
		}
		public async Task<EventDto> GetEventByNameAsync(string name)
		{
			var @event = await _unitOfWork.Events.FindAsync(e=>e.Name==name);
			if (@event == null)
				return null;

			var result = _mapper.Map<EventDto>(@event);
			return result;
		}
		public async Task<EventDto> CreateEventAsync(CreateEventDto createEventDto)
		{

			var @event = _mapper.Map<Event>(createEventDto);
			
			await _unitOfWork.Events.AddAsync(@event);
			await _unitOfWork.CompleteAsync();

			var result = _mapper.Map<EventDto>(@event);
			return result;

		}

		public async Task<EventDto> UpdateEventAsync(UpdateEventDto updateEventDto)
		{
			var @event = await _unitOfWork.Events.GetByIdAsync(updateEventDto.Id);
			if (@event == null)
				return null;

			@event.Name = updateEventDto.Name;
			@event.Description = updateEventDto.Description;
			@event.Category = updateEventDto.Category;
			@event.Date = updateEventDto.Date;
			@event.Venue = updateEventDto.Venue;
			@event.Price = updateEventDto.Price;
			@event.ImageUrl = updateEventDto.ImageUrl;
			@event.UpdatedAt = DateTime.UtcNow;

			_unitOfWork.Events.Update(@event);
			await _unitOfWork.CompleteAsync();
			var result = _mapper.Map<EventDto>(@event);
			return result;
			
		}

		public async Task<bool> DeleteEventAsync(int id)
		{
			var @event = await _unitOfWork.Events.GetByIdAsync(id);
			if (@event == null)
				return false;

			_unitOfWork.Events.Delete(@event);
			await _unitOfWork.CompleteAsync();
			return true;
		}

		public async Task<bool> EventExistsAsync(int id)
		{
			var @event = await _unitOfWork.Events.GetByIdAsync(id);
			return @event != null;
		}

		
	}
}
