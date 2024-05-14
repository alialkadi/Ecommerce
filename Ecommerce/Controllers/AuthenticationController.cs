using AutoMapper;
using Ecommerce.APIs.Dtos;
using Ecommerce.APIs.Errors;
using Ecommerce.APIs.Extentions;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ecommerce.APIs.Controllers
{
	
	public class AuthenticationController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;
		private readonly TokenValidationParameters _tokenValidationParameters;

		public AuthenticationController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager, ITokenService tokenService,
			IMapper mapper, IOptions<JwtBearerOptions> jwtOptions)
        {
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._tokenService = tokenService;
			this._mapper = mapper;
			_tokenValidationParameters = jwtOptions.Value.TokenValidationParameters;
		}
		// Register
		[HttpPost("Register")]
		public async Task<ActionResult<ResponseAuth>> Register(RegisterDto model)
		{
			if( checkEmailExist(model.Email).Result.Value )
			{
				return BadRequest(new ApiResponse(400, "Email Already Exist"));
			}

			var flag = await _userManager.FindByEmailAsync(model.Email);
			if (flag is null)
			{
				var user = new AppUser()
				{
					DisplayName = model.Name,
					Email = model.Email,
					PhoneNumber = model.Phone,
					UserName = model.Email.Split('@')[0],
					address = new Address()
					{
						FName = model.Fname,
						LName = model.Lname,
						City = model.City,
						Country = model.Country,
						Street = model.Street
					}

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
		[HttpGet("emailExists")]
		public async Task<ActionResult<bool>> checkEmailExist(string email)
		{
			var res =await _userManager.FindByEmailAsync(email);
			if (res == null) return false;
			return true;
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
					//if (!addToLogin.Succeeded)
					//{
					//	var errorsList = new List<string>();
					//	foreach (var error in addToLogin.Errors)
					//	{
					//		errorsList.Add(error.Description);
					//	}

					//	return BadRequest(new ApiResponse(400)
					//	{
					//		Errors = errorsList
					//	});

					//}

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
		[Authorize]
		[HttpGet("CurrentUser")]
		public async Task<ActionResult<ResponseAuth>> GetCurrentUser()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(userEmail);
			return Ok(new ResponseAuth()
			{
				status= new ApiResponse(200).Message,
				UserData = new UserDto()
				{
					Email = user.Email,
					DisplayName = user.DisplayName,
				},
				token = await _tokenService.CreateTokenAsync(user,_userManager),
			});
		}
		[Authorize]
		[HttpGet("CurrentUserAddress")]
		public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
		{
			//var userEmail = User.FindFirstValue(ClaimTypes.Email);
			//var user = await _userManager.FindByEmailAsync(userEmail);
			var user = await _userManager.FinduserWithAddressAsync(User);
			var mapped  = _mapper.Map<Address, AddressDto>(user.address);

			return Ok(mapped);
		}

		[Authorize]
		[HttpPut("UpdateAddress")]
		public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto address)
		{
			var user = await _userManager.FinduserWithAddressAsync(User);
			var mapped = _mapper.Map<AddressDto, Address>(address);
			user!.address = mapped;

			var res = await _userManager.UpdateAsync(user);
			if (!res.Succeeded)
			{
				var errors = res.Errors.Select(e => e.Description);
				var errorResponse = new ApiResponse(400, "Update Address failed")
				{
					Errors = errors.ToList() // Include errors for debugging
				};

				return BadRequest(errorResponse);
			}
			else
			{
				return Ok(address);
			}
				

		}
		
		[HttpPost("ForgetPassword")]
		public async Task<ActionResult<ForgetPassWordResponse>> ForgetPassword ([FromBody] ForgetPasswordDto model, [FromHeader] string Token)
		{
			var handler = new JwtSecurityTokenHandler();
			var Wtoken = handler.ReadJwtToken(Token);

			// Check if the token contains an "email" claim
			var emailClaim = Wtoken.Payload.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
			var Wemail = "";
			if (emailClaim != null)
			{
				Wemail = emailClaim.Value;
				
			}
			else
			{
				return Unauthorized(new ApiResponse(401, "Token problem"));

			}
			if (!Request.Headers.ContainsKey("Token"))
			{
				return Unauthorized(new ApiResponse(401, "Token header is missing"));
			}

			var user =await _userManager.FindByEmailAsync(model.Email);
			if(user is not  null)
			{
				
				if (Wemail != model.Email)
				{
					return BadRequest(new ApiResponse(400, "Logged In User Email doesn't Match Entered Email"));
				}
				Random random = new Random();
				int code = random.Next(100000, 999999); 
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				var body = code;
				var url = Url.Action("ResetPassword", "Account",
						new { email = model.Email, token = token }, Request.Scheme);
				var mail = new Email()
				{
					Recioient = model.Email,
					Subject = "Reset Your Password",
					Body = code + "\n" + url
				};
				EmailSettings.SendEmail(mail);
				var res = new ForgetPassWordResponse()
				{
					Email = model.Email,
					ResetCode = code,
					Token = token,
					Url = Wemail,
				};
				return Ok(res);
			}
			else
			{
				return BadRequest(new ApiResponse(400, "User Is not Found"));
			}
			
		}
		 
	}
}
