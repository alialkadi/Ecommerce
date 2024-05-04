using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Services.Interfaces
{
	public interface ITokenService
	{
		Task<string> CreateTokenAsync(AppUser appUser, UserManager<AppUser> userManager);

	}
}
