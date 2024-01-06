using krill_be.Models;
using krill_be.Services;
using Microsoft.AspNetCore.Mvc;

namespace krill_be.Controllers
{
	public class UserController : Controller
	{

		private readonly UserService _userService;

		public UserController(UserService userService)
		{
			_userService = userService;
		}

		[HttpGet("api/getUser", Name = "GetUser")]
		public async Task<User?> GetUser()
		{
			var response = await _userService.GetOneAsync();
			return response;
		}

		[HttpPost("api/register", Name = "RegisterUser")]
		public async Task<IActionResult> register([FromBody] User user)
		{
			if(user.areRegistrationFieldsCompiled())
			{
				var isUserFound = await _userService.checkIfUserExistByEmail(user.Email);

				if (!isUserFound)
				{
					await _userService.CreateOneAync(user);
					return Ok("User registered successfully");
				}
				else
				{
					return BadRequest("Email already associated with a user");
				}
			}
			else
			{
				return NotFound("Username and password are not filled correctly");
			}
		}

		[Route("api/login", Name = "Login")]
		[HttpPost]
		public void login() { }

		[Route("api/changePassword", Name = "ChangePassword")]
		[HttpPut]
		public async Task<IActionResult> changePassword([FromBody] string oldPassword) 
		{
			if(oldPassword == null) return NoContent();
			// servizio di match della password? quando uso i service?
			return NoContent(); 
		}
	}
}
