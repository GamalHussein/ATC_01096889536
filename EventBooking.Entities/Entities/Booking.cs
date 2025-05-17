namespace EventBooking.Domain.Entities
{
	public class Booking
	{
		public int Id { get; set; }
		public DateTime BookingDate { get; set; }
		public int TicketCount { get; set; }
		public bool IsConfirmed { get; set; }

		public string UserId { get; set; }
		public int EventId { get; set; }
		// Navigation properties
		public virtual Event Event { get; set; }
		public virtual ApplicationUser User { get; set; }
	}
}
