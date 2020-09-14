using System.Linq;
using BusinessLayer.InrefacesRepository;
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
        UserManager<User> userManager;
        private readonly ITrackingsRepository trackingsRepostiroy;

        public TrackingController(UserManager<User> _userManager, ITrackingsRepository trackingsRepostiroy)
        {
            userManager = _userManager;
            this.trackingsRepostiroy = trackingsRepostiroy;
        }

        //Добавления пользователя в список для отслеживание книг
        [HttpGet]
        public IActionResult Track(int bookId)
        {
            trackingsRepostiroy.Add(bookId, userManager.GetUserId(User));
            return RedirectToAction("ListBook", "Book");
        }

        //Уаление пользователя от списка 
        public IActionResult UnTrace(int bookId)
        {
            trackingsRepostiroy.Delete(bookId, userManager.GetUserId(User));
            return RedirectToAction("ListBook", "Book");
        }

    }
}