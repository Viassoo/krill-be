using krill_be.Models;
using krill_be.Services;
using krill_be.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace krill_be.Controllers
{
    public class UserController : Controller
	{

		private readonly UserService _userService;

		private readonly LoginSettings _loginSettings;
		public UserController(UserService userService, LoginSettings loginSettings)
		{
			_userService = userService;
			_loginSettings = loginSettings;
		}

		[HttpGet("api/getUser", Name = "GetUser")]
		public async Task<User?> GetUser()
		{
			var response = await _userService.GetOneAsync();
			return response;
		}

		[HttpPost("api/register", Name = "RegisterUser")]
		public async Task<IActionResult> register([FromBody] LoginData loginData)
		{
			if(loginData.AreRegistrationFieldsFilled())
			{
				var isEmailDomainElegible = true;
				var isUserFound = await _userService.checkIfUserExistByEmail(loginData.Email);

				if(isEmailDomainElegible)
				{
					if (!isUserFound)
					{
						Hashing hash = new Hashing();
						User user = new User();
						
						user.Email = loginData.Email;
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
		public async Task<IActionResult> Login([FromBody] LoginData loginData)
		{
			try
			{
				if (loginData.AreRegistrationFieldsFilled())
				{
					User? userFound = await _userService.GetUserByEmail(loginData.Email);
					if (userFound != null)
					{
						Hashing hashing = new Hashing();
						if (hashing.VerifyHash(loginData.Password, userFound.Password, userFound.Salt))
						{
							List<Claim> claims = new List<Claim>
						{
							new Claim(ClaimTypes.Name, userFound.Email),
							// TODO -> passare poi il ruolo
							new Claim(ClaimTypes.Role, "User")
						};

							var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

							var authProperties = new AuthenticationProperties
							{
								AllowRefresh = true,
								ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(_loginSettings.cookieMinutesExpireTime),
								IsPersistent = true,

							};

							await HttpContext.SignInAsync(
								CookieAuthenticationDefaults.AuthenticationScheme,
								new ClaimsPrincipal(claimsIdentity)
								);

							return Ok();
						}
						else
						{
							return NotFound("Login failed");
						}
					}
					else
					{
						return BadRequest("User does not exists");
					}
				}
				else
				{
					string message;
					if (loginData.Email == null || loginData.Email == "")
					{
						message = "email field not filled correctly";
					}
					else
					{
						message = "password field not filled correctly";
					}
					return BadRequest(message);
				}

			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}	

		}

		[Route("api/logout", Name = "Logout")]
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok("Logged out correctly");
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
