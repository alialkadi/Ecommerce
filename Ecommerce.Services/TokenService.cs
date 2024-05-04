using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.Services.Interfaces;
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

namespace Ecommerce.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService( IConfiguration configuration )
        {
			this._configuration = configuration;
		}
        public async Task<string> CreateTokenAsync(AppUser appUser, UserManager<AppUser> userManager )
		{
			//Header

			//Payload
			// private Claims [from the user : name, email, id.....]

			var AuthClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.GivenName,appUser.DisplayName),
				new Claim(ClaimTypes.Email,appUser.Email),
			};
			var roles =await userManager.GetRolesAsync(appUser);
			foreach (var role in roles)
			{
				AuthClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			//Signature
			// key
			var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
			var token = new JwtSecurityToken(
				issuer : _configuration["JWT:ValidIssuer"],
				audience : _configuration["JWT:ValidAudience"],
				claims : AuthClaims,
				expires: DateTime.Now.AddDays(double.Parse( _configuration["JWT:Duration"] )),
				signingCredentials : new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
