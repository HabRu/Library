using Library.Models;
using Library.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Library.Services.RoleControlServices;

namespace Library.Controllers
{
    //Администрирование
    [Authorize(Roles = RolesConfig.admin)]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IRoleControlService roleControl;

        public RolesController(RoleManager<IdentityRole> roleManager,
                               UserManager<User> userManager,
                               IRoleControlService roleControl)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this.roleControl = roleControl;
        }
        public IActionResult Index() => View(_roleManager.Roles.ToList());
        public IActionResult Create() => View();
        //Создание нового пользователя
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            #region Старая реализация
            //if (!string.IsNullOrEmpty(name))
            //{
            //    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
            //    if (result.Succeeded)
            //    {
            //        return RedirectToAction("Index");
            //    }
            //    else
            //    {
            //        foreach (var error in result.Errors)
            //        {
            //            ModelState.AddModelError(string.Empty, error.Description);
            //        }
            //    }
            //}
            //return View(name); 
            #endregion
            return await roleControl.Create(name, this);
        }

        //Удаление пользователя
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            #region Старая реализация
            //IdentityRole role = await _roleManager.FindByIdAsync(id);
            //if (role != null)
            //{
            //    IdentityResult result = await _roleManager.DeleteAsync(role);
            //}
            //return RedirectToAction("Index"); 
            #endregion
            return await roleControl.Delete(id, this);
        }
        //Список пользователей
        public IActionResult UserList() => View(_userManager.Users.ToList());
        public async Task<IActionResult> Edit(string userId)
        {
            #region Старая реализация
            //// получаем пользователя
            //User user = await _userManager.FindByIdAsync(userId);
            //if (user != null)
            //{
            //    // получем список ролей пользователя
            //    var userRoles = await _userManager.GetRolesAsync(user);
            //    var allRoles = _roleManager.Roles.ToList();
            //    ChangeRoleViewModel model = new ChangeRoleViewModel
            //    {
            //        UserId = user.Id,
            //        UserEmail = user.Email,
            //        UserRoles = userRoles,
            //        AllRoles = allRoles
            //    };
            //    return View(model);
            //}

            //return NotFound(); 
            #endregion
            return await roleControl.Edit(userId, this);
        }

        //Редактирование ролей пользователя
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            #region Старая реализация
            //// получаем пользователя
            //User user = await _userManager.FindByIdAsync(userId);
            //if (user != null)
            //{
            //    // получем список ролей пользователя
            //    var userRoles = await _userManager.GetRolesAsync(user);
            //    // получаем все роли
            //    var allRoles = _roleManager.Roles.ToList();
            //    // получаем список ролей, которые были добавлены
            //    var addedRoles = roles.Except(userRoles);
            //    // получаем роли, которые были удалены
            //    var removedRoles = userRoles.Except(roles);

            //    await _userManager.AddToRolesAsync(user, addedRoles);

            //    await _userManager.RemoveFromRolesAsync(user, removedRoles);

            //    return RedirectToAction("UserList");
            //}

            //return NotFound(); 
            #endregion
            return await roleControl.Edit(userId, roles, this);
        }
    }
}
