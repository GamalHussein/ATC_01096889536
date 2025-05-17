using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Dtos
{
	public class BookingDto
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public int EventId { get; set; }
		public string EventName { get; set; }
		public DateTime BookingDate { get; set; }
		public int TicketCount { get; set; }
		public bool IsConfirmed { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
