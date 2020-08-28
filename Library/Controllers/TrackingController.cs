using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    //Контроллер для отслеживание бронированных книг
    [Authorize]
    public class TrackingController : Controller
    {
        ApplicationContext db;
        UserManager<User> userManager;

        public TrackingController(ApplicationContext _db, UserManager<User> _userManager)
        {
            this.db = _db;
            userManager = _userManager;
        }


      //Добавления пользователя в список для отслеживание книг
        [HttpGet]
        public IActionResult Track(int bookId)
        {
            Tracking tracking = new Tracking
            {
                BookId = bookId,
                UserId = userManager.GetUserId(User)
            };
            db.Trackings.Add(tracking);
            db.SaveChanges();
            return RedirectToAction("ListBook", "Book");
        }
        //Уаление пользователя от списка 
        public IActionResult UnTrace(int bookId)
        {
            Tracking tracking = db.Trackings.FirstOrDefault((t) =>t.BookId == bookId && t.UserId == userManager.GetUserId(User));
            db.Trackings.Remove(tracking);
            db.SaveChanges();
            return RedirectToAction("ListBook", "Book");
        }

    }
}