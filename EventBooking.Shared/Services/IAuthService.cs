using EventBooking.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Services
{
	public interface IAuthService
	{
		Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
		Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto);
	}
}
