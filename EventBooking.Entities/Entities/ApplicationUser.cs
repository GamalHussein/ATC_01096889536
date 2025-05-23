﻿

using Microsoft.AspNetCore.Identity;

namespace EventBooking.Domain.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		// Navigation properties
		public virtual ICollection<Booking> Bookings { get; set; }
	}
}
