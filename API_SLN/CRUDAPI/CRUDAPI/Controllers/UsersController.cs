using CRUDAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUDAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UsersController : Controller
	{
		private readonly UserRepository _userRepository;

		public UsersController(UserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[HttpGet]
		[Route("exportData")]
		public async Task<IActionResult> GetAll()
		{
			var users = await _userRepository.GetAllUsers();
			return Ok(users);
		}
	}
}