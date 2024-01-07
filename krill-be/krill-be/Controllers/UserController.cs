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
			if(user.AreRegistrationFieldsFilled())
			{
				var isEmailDomainElegible = true;
				var isUserFound = await _userService.checkIfUserExistByEmail(user.Email);

				if(isEmailDomainElegible)
				{
					if (!isUserFound)
					{
						Hashing hash = new Hashing();
						user.Password = hash.Hash(user.Password, out var salt);
						user.Salt = salt;
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
					return BadRequest("Email domain is not elegible");
				}
			}
			else
			{
				return NotFound("Username and password are not filled correctly");
			}
		}

		[Route("api/login", Name = "Login")]
		[HttpPost]
		public async Task<IActionResult> login([FromBody] User user)
		{
			if(user.AreRegistrationFieldsFilled())
			{
				User? userFound = await _userService.GetUserByEmail(user.Email);
				if(userFound != null)
				{
					Hashing hashing = new Hashing();
					if (hashing.VerifyHash(user.Password, userFound.Password, userFound.Salt))
					{
						return Ok("Login eseguito");
					}
					else
					{
						return NotFound("Impossible to login"); 
					}
				}
				else
				{
					return BadRequest("User does not exists");
				}
			}
			else
			{
				return BadRequest("Not all fields are filled");
			}
		}

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
