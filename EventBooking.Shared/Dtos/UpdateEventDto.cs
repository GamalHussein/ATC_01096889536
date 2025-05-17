using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Dtos
{
	public class UpdateEventDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		[StringLength(50)]
		public string Category { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		[StringLength(100)]
		public string Venue { get; set; }

		[Required]
		[Range(0, 10000)]
		public decimal Price { get; set; }

		[StringLength(500)]
		public string ImageUrl { get; set; }
	}
}
