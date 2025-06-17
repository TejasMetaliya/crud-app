using CRUDAPI.Models;
using CRUDAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CRUDAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private static Logger _logger = LogManager.GetCurrentClassLogger();
		private readonly AuthService _authService;

		public AuthController(AuthService authService)
		{
			_authService = authService;
		}

		// Login endpoint
		[HttpPost("login")]
		public async Task<ActionResult<AuthResponse>> Login([FromBody] Users user)
		{
			try
			{
				if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
				{
					return BadRequest(new AuthResponse
					{
						IsSuccess = false,
						Token = "",
						Expiration = null,
						ErrorMessage = "Username and password are required"
					});
				}

				var isValid = await _authService.ValidateUserCredentials(user.Name, user.Password);
				if (!isValid)
				{
					return Unauthorized(new AuthResponse
					{
						IsSuccess = false,
						Token = "",
						Expiration = null,
						ErrorMessage = "Invalid username or password"
					});
				}

				var token = _authService.GenerateJwtToken(user.Name);
				return Ok(new AuthResponse
				{
					IsSuccess = true,
					Token = token,
					Expiration = DateTime.Now.AddMinutes(30),
					ErrorMessage = ""
				});
			}
			catch (Exception e)
			{
				_logger.Error(e, "An error occurred during login for user: {Username}", user?.Name);
				return StatusCode(500, new AuthResponse
				{
					IsSuccess = false,
					Token = "",
					Expiration = null,
					ErrorMessage = "An unexpected error occurred. Please try again later."
				});
			}
		}
	}
}