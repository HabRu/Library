using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace Library.Controllers
{
    [Authorize]
    public class UserPageController:Controller
    {
        UserManager<User> _userManager;
        ApplicationContext db;
        public UserPageController(ApplicationContext applicationContext,UserManager<User> user)
        {
            db = applicationContext;
            _userManager = user;
        }
        public IActionResult MyPage()
        {
            User user =db.Users.FirstOrDefault(p=>p.Id==_userManager.GetUserId(User));
            user.ReservUser = db.Reservations.Where(p => p.User == user).ToList();
            return View(user);
        }
    }
}
