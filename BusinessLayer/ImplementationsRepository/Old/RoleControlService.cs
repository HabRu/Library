using Library.Models;
using Library.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.RoleControlServices
{
    public class RoleControlService : IRoleControlService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleControlService(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager) 
        {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
        }
        public async Task<IActionResult> Create(string name, Controller controller)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return controller.RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        controller.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return controller.View(name);
        }

        public async Task<IActionResult> Delete(string id, Controller controller)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return controller.RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string userId, Controller controller)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return controller.View(model);
            }

            return controller.NotFound();
        }

        public async Task<IActionResult> Edit(string userId, List<string> roles, Controller controller)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return controller.RedirectToAction("UserList");
            }

            return controller.NotFound();
        }
    }
}
