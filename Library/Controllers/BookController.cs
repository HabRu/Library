using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Library.Services.BookContorlServices;
using Microsoft.AspNetCore.Hosting;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly IBooksRepository bookControl;

        private readonly IWebHostEnvironment env;

        public BookController(IBooksRepository bookControl, IWebHostEnvironment env)
        {
            this.bookControl = bookControl;
            this.env = env;
        }

        //Get-контроллер.Возрат страницы для добавления книг
        //ДОСТУПНА ТОЛЬКО ДЛЯ РОЛИ "librarian"
        [Authorize(Roles = RolesConfig.librarian)]
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
        [Authorize(Roles = RolesConfig.librarian)]
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
            return View(bookControl.ListBook(model));
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
            await bookControl.AddComment(comment, User.Identity.Name);
            return RedirectToAction("GetThisBook", new { id = comment.BookId });
        }

        //Post-контроллер.Добавление оценки
        public async Task<IActionResult> AddEvaluation(EvaluationViewModel evaluation)
        {
            await bookControl.AddEvaluation(evaluation);
            return RedirectToAction("GetThisBook", new { id = evaluation.BookId });
        }

        [Authorize(Roles = RolesConfig.librarian)]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return View(bookControl.Edit(id));
        }

        //Post-контроллер. Редактирование информации о книг
        [HttpPost]
        [Authorize(Roles = RolesConfig.librarian)]
        public IActionResult Edit(EditBookViewModel edit)
        {
            bookControl.Edit(edit);
            return RedirectToAction("GetThisBook", new { id = edit.Id });
        }

    }
}
