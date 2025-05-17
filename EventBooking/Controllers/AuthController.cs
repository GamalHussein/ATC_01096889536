using EventBooking.Application.Dtos;
using EventBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventBooking.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.RegisterAsync(registerDto);
			if (!result.IsSuccess)
				return BadRequest(new { Errors = result.Errors });

			return Ok(result);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.LoginAsync(loginDto);
			if (!result.IsSuccess)
				return BadRequest(new { Errors = result.Errors });

			return Ok(result);
		}
	}
}

