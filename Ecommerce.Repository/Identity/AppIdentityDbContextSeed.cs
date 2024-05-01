using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repository.Identity
{
	public static class AppIdentityDbContextSeed
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if(userManager.Users.Count() == 0)
			{
				var user = new AppUser()
				{
					DisplayName = "Ali Alkady",
					Email = "alialkady360@gmail.com",
					UserName = "ali.alkady",
					PhoneNumber = "01018062078"
				};
				await userManager.CreateAsync(user, "Pa$$w0rd");
			}
		}
	}
}
