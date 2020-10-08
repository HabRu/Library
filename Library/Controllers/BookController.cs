using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Library.Services.BookContorlServices;
using Microsoft.AspNetCore.Hosting;
using BusinessLayer.InrefacesRepository;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly IBooksRepository bookControl;

        private readonly IWebHostEnvironment env;

        private readonly ITrackingsRepository trackings;

        private readonly UserManager<User> userManager;

        public BookController(IBooksRepository bookControl, IWebHostEnvironment env, ITrackingsRepository trackings, UserManager<User> userManager)
        {
            this.bookControl = bookControl;
            this.env = env;
            this.trackings = trackings;
            this.userManager = userManager;
        }

        //Get-контроллер.Возрат страницы для добавления книг
        //ДОСТУПНА ТОЛЬКО ДЛЯ РОЛИ "librarian"
        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        //Post-контроллер.Обработка формы для добавления книг
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookViewModel model)
        {
            await bookControl.AddBook(model, env.WebRootPath);
            //Возврат в начальную страницу
            return RedirectToAction("Index", "Home");

        }

        //Контоллер для удаление книг по id
        //ДОСТУПНА ТОЛЬКО ДЛЯ РОЛИ "librarian"
        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        [HttpGet]
        public async Task<IActionResult> DeleteBook(int? id)
        {
            await bookControl.DeleteBook(id);
            return RedirectToAction("ListBook");
        }

        //Контроллер для возрата книг(фильтрация по имени, языку, автору и жанру; пагинация)
        [AllowAnonymous]
        [Authorize]
        public IActionResult ListBook(BookFilterModel model)
        {
            var modelBooks = bookControl.ListBook(model);
            if (User.Identity.IsAuthenticated)
            {
                modelBooks.Trackings = trackings.GetTrackingsByUserId(userManager.GetUserId(User)).ToList();
                ViewBag.userId = userManager.GetUserId(User);
            }

            return View(modelBooks);
        }

        //Get-контроллер. Для получения одной книги по id
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetThisBook(int? id)
        {
            return View(await bookControl.GetThisBook(id));

        }

        //Post-контроллер.Добавление комментария
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel comment, string UserName)
        {
            await bookControl.AddComment(comment, UserName);
            return RedirectToAction("GetThisBook", new { id = comment.BookId });
        }

        //Post-контроллер.Добавление оценки
        public async Task<IActionResult> AddEvaluation(EvaluationViewModel evaluation)
        {
            await bookControl.AddEvaluation(evaluation);
            return RedirectToAction("GetThisBook", new { id = evaluation.BookId });
        }

        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return View(bookControl.Edit(id));
        }

        //Post-контроллер. Редактирование информации о книг
        [HttpPost]
        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        public IActionResult Edit(EditBookViewModel edit)
        {
            bookControl.Edit(edit);
            return RedirectToAction("GetThisBook", new { id = edit.Id });
        }

    }
}
