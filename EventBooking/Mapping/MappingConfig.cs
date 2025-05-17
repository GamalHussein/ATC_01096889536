using EventBooking.Application.Dtos;
using EventBooking.Domain.Entities;
using Mapster;
namespace EventBooking.Api.Mapping
{
	

	public class MappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<RegisterUserDto, ApplicationUser>()
	            .Map(dest => dest.UserName, src => src.Email)
	            .Map(dest => dest.Email, src => src.Email)
	            .Map(dest => dest.FirstName, src => src.FirstName)
	            .Map(dest => dest.LastName, src => src.LastName);

			config.NewConfig<Booking, BookingDto>()
				.Map(dest => dest.EventName, src => src.Event.Name)
				.Map(dest => dest.TotalPrice, src => src.TicketCount * src.Event.Price);

			config.NewConfig<BookingDto, Booking>()
				.Map(dest => dest.Id, src => src.Id)
				.Map(dest => dest.Event.Name, src => src.EventName);
				


			config.NewConfig<CreateBookingDto, Booking>();

			config.NewConfig<Event, EventDto>()
				.Map(dest => dest.IsBooked, src => false);

			config.NewConfig<Event, EventDetailDto>();

			config.NewConfig<CreateEventDto, Event>();

			config.NewConfig<UpdateEventDto, Event>();
		}
	}

}
