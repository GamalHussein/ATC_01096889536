using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Dtos
{
	public class CreateBookingDto
	{
		[Required]
		public int EventId { get; set; }

		[Required]
		[Range(1, 10)]
		public int TicketCount { get; set; }
	}
}
