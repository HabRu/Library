using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Library.Services.BookContorlServices;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookControlService bookControl;

        public BookController(IBookControlService bookControl)
        {
            this.bookControl = bookControl;
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
            #region
            ////Создаем переменную книги
            //Book book = new Book { Id = model.Id, Title = model.Title, Language = model.Language, Authtor = model.Authtor, Year = model.Year, Publisher = model.Publisher, Genre = model.Genre, Status = Status.Available };
            ////Если изображение не null,
            //if (model.Image != null)
            //{

            //    var path = "/images/" + model.Image.FileName;
            //    var contentPath = hostEnvironment.WebRootPath + path;

            //    if (!System.IO.File.Exists(contentPath))
            //    {
            //        using (var fileStream = new FileStream(contentPath, FileMode.Create))
            //        {
            //            await model.Image.CopyToAsync(fileStream);
            //        }
            //    }

            //    book.Image = path;
            //}

            ////Добавляем книгу в бд
            //db.Books.Add(book);
            ////Сохраняем изменения
            //await db.SaveChangesAsync();
            #endregion
            await bookControl.AddBook(model);
            //Возврат в начальную страницу
            return RedirectToAction("Index", "Home");

        }

        //Контоллер для удаление книг по id
        //ДОСТУПНА ТОЛЬКО ДЛЯ РОЛИ "librarian"
        [Authorize(Roles = RolesConfig.librarian)]
        [HttpGet]
        public async Task<IActionResult> DeleteBook(int? id)
        {
            #region Старая реализация
            //Book book = await db.Books.FirstOrDefaultAsync(p => p.Id == id);
            //if (book != null)
            //{
            //    db.Books.Remove(book);
            //    await db.SaveChangesAsync();
            //}
            #endregion
            await bookControl.DeleteBook(id);
            return RedirectToAction("ListBook");
        }

        //Контроллер для возрата книг(фильтрация по имени, языку, автору и жанру; пагинация)
        [AllowAnonymous]
        [Authorize]
        public IActionResult ListBook(BookFilterModel model)
        {
            #region Старая реализация
            //int pageSize = 7;

            //IQueryable<Book> Books = db.Books.Include(b => b.TrackingList).ThenInclude(t => t.User);

            ////BEGIN: Конвейр фильтраиции
            //if (title != null)
            //{
            //    Books = Books.Where(p => p.Title.StartsWith(title));
            //}
            //if (language != null)
            //{
            //    Books = Books.Where(p => p.Language.StartsWith(language));
            //}
            //if (author != null)
            //{
            //    Books = Books.Where(p => p.Authtor.StartsWith(author));
            //}
            //if (genre != null)
            //{
            //    Books = Books.Where(p => p.Genre.StartsWith(genre));
            //}
            //if (publisher != null)
            //{
            //    Books = Books.Where(p => p.Publisher.StartsWith(publisher));
            //}
            ////END: Конвейр фильтраиции

            ////Сортировка книг
            //switch (sortOrder)
            //{
            //    case SortState.NameDesc:
            //        Books = Books.OrderByDescending(s => s.Title);
            //        break;
            //    case SortState.AuthorAsc:
            //        Books = Books.OrderBy(s => s.Authtor);
            //        break;
            //    case SortState.AuthorDesc:
            //        Books = Books.OrderByDescending(s => s.Authtor);
            //        break;
            //    case SortState.LangAsc:
            //        Books = Books.OrderBy(s => s.Language);
            //        break;
            //    case SortState.LangDesc:
            //        Books = Books.OrderByDescending(s => s.Language);
            //        break;
            //    case SortState.PubAsc:
            //        Books = Books.OrderBy(s => s.Publisher);
            //        break;
            //    case SortState.PubDesc:
            //        Books = Books.OrderByDescending(s => s.Publisher);
            //        break;
            //    default:
            //        Books = Books.OrderBy(s => s.Title);
            //        break;
            //}

            //var count = await Books.CountAsync();
            //var items = await Books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            //// формируем модель представления
            //AllListBookViewModel viewModel = new AllListBookViewModel
            //{
            //    PageViewModel = new PageViewModel(count, page, pageSize),
            //    SortViewModel = new SortViewModel(sortOrder),
            //    FilterViewModel = new FilterViewModel(title, language, author, genre, publisher),
            //    Books = items
            //};\
            #endregion
            return View(bookControl.ListBook(model));
        }

        //Get-контроллер. Для получения одной книги по id
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetThisBook(int? id)
        {
            #region Старая реализация
            //Book book = await db.Books.Include(b => b.Comments).Include(b => b.Evaluation).Include(b => b.Evaluation).FirstOrDefaultAsync(p => p.Id == id);
            //List<Comment> comment = book.Comments.ToList();

            //if (comment != null)
            //{
            //    //Сорртируем крментарри по времени
            //    comment.OrderBy(c => c.Id);
            //    book.Comments = comment;
            //}
            #endregion
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
