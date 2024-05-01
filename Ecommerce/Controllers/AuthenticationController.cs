using Ecommerce.APIs.Dtos;
using Ecommerce.APIs.Errors;
using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.APIs.Controllers
{
	
	public class AuthenticationController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AuthenticationController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager )
        {
			this._userManager = userManager;
			this._signInManager = signInManager;
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
					token = "this placeholder for token"
				};
				return Ok(response);
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

				var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded)
                {
					return Unauthorized(new ApiResponse(401));

				}
				else
				{
					string name = user.DisplayName; 
					
					var response = new ResponseAuth(name,Email:model.Email  ,"")
					{
						status = new ApiResponse(200).Message,
						token = "this placeholder for token"
					};
					return Ok(response);
				}
			}
			else
			{
				return Unauthorized( new ApiResponse(401));
			}
		}
	}
}
