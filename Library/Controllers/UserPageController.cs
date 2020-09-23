using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Authorize]
    public class UserPageController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly ApplicationContext db;
        public UserPageController(ApplicationContext applicationContext, UserManager<User> user)
        {
            db = applicationContext;
            _userManager = user;
        }

        public IActionResult MyPage()
        {
            var user = db.Users.Include(u => u.ReservUser).FirstOrDefault(p => p.Id == _userManager.GetUserId(User));
            return View(user);
        }
    }
}
