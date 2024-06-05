using AdminDashboard.Models;
using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AdminDashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController( UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager )
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                Name = u.DisplayName,
                Email = u.Email,
                isConfirmed = u.EmailConfirmed
            }).ToListAsync();

            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);
                user.Roles = await _userManager.GetRolesAsync(appUser);
            }

            return View(users);
        }

        public async Task<IActionResult> Edit( string id )
        {
            var user =await _userManager.FindByIdAsync( id );
            var Roles = await _roleManager.Roles.ToListAsync();

            if (user != null)
            {
                var viewModel = new UserRoleViewModel()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Roles = Roles.Select(r => new RoleViewModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                    }).ToList()
                };
                return View(viewModel);
            }
            else
            {
                 ModelState.AddModelError("All", "User Not Found");
                return View("index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var uRoles = await  _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (uRoles.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user,role.Name);
                if(!uRoles.Any(u => u == role.Name) && role.IsSelected) 
                    await _userManager.AddToRoleAsync(user,role.Name);
            }
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID is null");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User Was not Found");

            var del = await _userManager.DeleteAsync(user);
            if (del.Succeeded)
            {
                
                return RedirectToAction("Index");
            }

            foreach (var error in del.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

    }
}
