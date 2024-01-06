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

		[HttpGet]
		public void getUsers()
		{
		}

		[HttpGet("api/getuser", Name = "GetUser")]
		public async Task<User?> GetUser()
		{
			var response = _userService.GetOneAsync();
			return response.Result;
		}

		[HttpPost]
		public async Task<IActionResult> register([FromBody] User user)
		{
			if(user.areRegistrationFieldsCompiled())
			{
				return Ok();
			}
			else
			{
				return NoContent();
			}
		}

		[HttpPost]
		public void login() { }

		[HttpPut]
		public async Task<IActionResult> changePassword([FromBody] string oldPassword) 
		{
			if(oldPassword == null) return NoContent();
			// servizio di match della password? quando uso i service?
			return NoContent(); 
		}
	}
}
