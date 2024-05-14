using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.APIs.Extentions
{
	public static class UserManagerExtensions
	{

		public static async Task<AppUser?> FinduserWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal user)
		{
			var userEmail = user.FindFirstValue(ClaimTypes.Email);
			var UserWithAddress = await userManager.Users.Include(u => u.address).FirstOrDefaultAsync(c => c.Email == userEmail);

			return UserWithAddress;
		}
	}
}
