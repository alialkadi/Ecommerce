using Ecommerce.APIs.Dtos;
using Ecommerce.APIs.Errors;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.APIs.Controllers
{
	
	public class AuthenticationController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;

		public AuthenticationController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager, ITokenService tokenService )
        {
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._tokenService = tokenService;
		}
		// Register

		[HttpPost("Register")]
		public async Task<ActionResult<ResponseAuth>> Register(RegisterDto model)
		{
			

			var flag = await _userManager.FindByEmailAsync(model.Email);
			if (flag is null)
			{
				var user = new AppUser()
				{
					DisplayName = model.Name,
					Email = model.Email,
					PhoneNumber = model.Phone,
					UserName = model.Email.Split('@')[0],

				};

				var result = await _userManager.CreateAsync(user, model.Password);
				if (!result.Succeeded)
				{
					var errors = result.Errors.Select(e => e.Description);
					var errorResponse = new ApiResponse(400, "Registration failed")
					{
						Errors = errors.ToList() // Include errors for debugging
					};

					return BadRequest(errorResponse);
				}
				
				var response = new ResponseAuth(model.Name, model.Email, "")
				{
					status = new ApiResponse(200).Message,
					token = await _tokenService.CreateTokenAsync(user, _userManager),

				};
				return Created(Uri.UriSchemeHttp, response);
			}
			else
			{
				return BadRequest(new ApiResponse(400, "User Email Already exist"));
			}

		}

		// login


		[HttpPost("Login")]
		public async Task<ActionResult<ResponseAuth>> Login(LoginDto model)
		{

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is not null)
			{
				var res = await HttpContext.AuthenticateAsync();

				var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

				if (!result.Succeeded)
				{
					return Unauthorized(new ApiResponse(401));

				}
				else
				{
					var loginProvider = user.Email;
					var userIdClaim = user.Id;
					var displayNameClaim = user.DisplayName;
					var addToLogin = await _userManager.AddLoginAsync(user,
									new UserLoginInfo(loginProvider, userIdClaim, displayNameClaim));
					if (!addToLogin.Succeeded)
					{
						var errorsList = new List<string>();
						foreach (var error in addToLogin.Errors)
						{
							errorsList.Add(error.Description);
						}

						return BadRequest(new ApiResponse(400)
						{
							Errors = errorsList
						});

					}

					string name = user.DisplayName;

					var response = new ResponseAuth(name, Email: model.Email, "")
					{
						status = new ApiResponse(200).Message,
						token =await _tokenService.CreateTokenAsync(user,_userManager),
					};
					return Ok(response);
				}
			}
			else
			{
				return Unauthorized(new ApiResponse(401));
			}
		}
	}
}
