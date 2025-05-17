using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Dtos
{
	public class EventDetailDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public DateTime Date { get; set; }
		public string Venue { get; set; }
		public decimal Price { get; set; }
		public string ImageUrl { get; set; }
	}
}
