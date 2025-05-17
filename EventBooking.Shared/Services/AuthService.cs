using EventBooking.Application.Dtos;
using EventBooking.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Application.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;

		public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration , IMapper mapper)
		{
			_userManager = userManager;
			_configuration = configuration;
			_mapper = mapper;
		}

		public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
		{
			var response = new AuthResponseDto
			{
				IsSuccess = false,
				Errors = new List<string>()
			};

			var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
			if (userExists != null)
			{
				response.Errors.Add("Email is already registered.");
				return response;
			}

			var newUser = _mapper.Map<ApplicationUser>(registerDto);

			

			var result = await _userManager.CreateAsync(newUser, registerDto.Password);
			if (!result.Succeeded)
			{
				response.Errors.AddRange(result.Errors.Select(e => e.Description));
				return response;
			}

			// Add to User role by default
			await _userManager.AddToRoleAsync(newUser, Roles.User);

			// Generate token for auto-login
			var token = await GenerateJwtToken(newUser);

			response.IsSuccess = true;
			response.UserId = newUser.Id;
			response.Email = newUser.Email;
			response.Token = token;
			response.Roles = new List<string> { Roles.User };


			return response;
		}

		public async Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto)
		{
			var response = new AuthResponseDto
			{
				IsSuccess = false,
				Errors = new List<string>()
			};

			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (user == null)
			{
				response.Errors.Add("Invalid email or password.");
				return response;
			}

			var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
			if (!isPasswordValid)
			{
				response.Errors.Add("Invalid email or password.");
				return response;
			}

			var token = await GenerateJwtToken(user);
			var roles = await _userManager.GetRolesAsync(user);

			response.IsSuccess = true;
			response.UserId = user.Id;
			response.Email = user.Email;
			response.Token = token;
			response.Roles = roles.ToList();

			return response;
		}

		private async Task<string> GenerateJwtToken(ApplicationUser user)
		{
			var userRoles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			foreach (var role in userRoles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			//var jwtKey = Environment.GetEnvironmentVariable("EVENT_BOOKING_SECRET");

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("alsdasdasdas455sdfsdfsd#sdfsdf5564sdfsdfsdfsllmsmdfsdfnmnsdfsdfsdscxcsdlsdfsdf"));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:issuer"],
				audience: _configuration["JwtSettings:audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
