using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Dtos
{
	public class AuthResponseDto
	{
		public bool IsSuccess { get; set; }
		public string Token { get; set; }
		public string UserId { get; set; }
		public string Email { get; set; }
		public List<string> Roles { get; set; }
		public List<string> Errors { get; set; }
	}
}
