using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.ViewModels;
using System.IO;
using Library.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace Library.Controllers
{
    public class BookController:Controller
    {
        ApplicationContext db;
        public BookController(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }
        //Get-контроллер.Возрат страницы для добавления книг
        //ДОСТУПНА ТОЛЬКО ДЛЯ РОЛИ "librarian"
        [Authorize(Roles = "librarian")]
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }
        //Post-контроллер.Обработка формы для добавления книг
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookViewModel model)
        {
            //Создаем переменную книги
            Book book = new Book { Id = model.Id, Title = model.Title, Language = model.Language, Authtor = model.Authtor, Year = model.Year,Publisher=model.Publisher, Genre = model.Genre,Status=Status.Естьвналичии };
            //Если изображение не null,
            if (model.Image != null)
            {
                //То сохраняем его в бд
                byte[] imageData = null;
                using(var binaryReader=new BinaryReader(model.Image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.Image.Length);
                }
                book.Image = imageData;
            }

            //Добавляем книгу в бд
            db.Books.Add(book);
            //Сохраняем изменения
            await db.SaveChangesAsync();
            //Возврат в начальную страницу
            return RedirectToAction("Index","Home");

        }

        //Контоллер для удаление книг по id
        //ДОСТУПНА ТОЛЬКО ДЛЯ РОЛИ "librarian"
        [Authorize(Roles = "librarian")]
        [HttpGet]
        public async Task<IActionResult> DeleteBook(int? id)
        {
            
            Book book = await db.Books.FirstOrDefaultAsync(p => p.Id == id);
            if (book != null)
            {
                db.Books.Remove(book);
              await  db.SaveChangesAsync();
            }
                return RedirectToAction("ListBook");  
        }

        //Контроллер для возрата книг(фильтрация по имени,языку,автору и жанру; пагинация)
        [AllowAnonymous]
        [Authorize]
       public async Task<IActionResult> ListBook(string title,string language,string author,string genre,string publisher, int page=1,SortState sortOrder=SortState.NameAsc) 
        {
            
            int pageSize = 7;

            IQueryable<Book> Books = db.Books.Include(b=>b.TrackingList).ThenInclude(t=>t.User);

            //BEGIN: Конвейр фильтраиции
            if (title != null)
            {
                Books = Books.Where(p => p.Title.StartsWith(title));
            }
            if (language != null)
            {
                Books = Books.Where(p => p.Language.StartsWith(language));
            }
            if (author != null)
            {
                Books = Books.Where(p => p.Authtor.StartsWith(author));
            }
            if (genre != null)
            {
                Books = Books.Where(p => p.Genre.StartsWith(genre));
            }
            if (publisher!=null)
            {
                Books = Books.Where(p => p.Publisher.StartsWith(publisher));
            }
            //END: Конвейр фильтраиции

            //Сортировка книг
            switch (sortOrder)
            {
                case SortState.NameDesc:
                     Books= Books.OrderByDescending(s => s.Title);
                    break;
                case SortState.AuthorAsc:
                    Books = Books.OrderBy(s => s.Authtor);
                    break;
                case SortState.AuthorDesc:
                    Books = Books.OrderByDescending(s => s.Authtor);
                    break;
                case SortState.LangAsc:
                    Books = Books.OrderBy(s => s.Language);
                    break;
                case SortState.LangDesc:
                    Books = Books.OrderByDescending(s => s.Language);
                    break;
                case SortState.PubAsc:
                    Books = Books.OrderBy(s => s.Publisher);
                    break;
                case SortState.PubDesc:
                    Books = Books.OrderByDescending(s => s.Publisher);
                    break;                    
                default:
                    Books = Books.OrderBy(s => s.Title);
                    break;
            }

            var count = await Books.CountAsync();
            var items = await Books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            AllListBookViewModel viewModel = new AllListBookViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(title,language,author,genre,publisher),
                Books= items
            };
            return View(viewModel);
        }

        //Get-контроллер. Для получения одной книги по id
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetThisBook(int? id)
        {
            
            Book book =await db.Books.Include(b=>b.Comments).Include(b=>b.Evaluation).Include(b=>b.Evaluation).FirstOrDefaultAsync(p => p.Id == id);
            List<Comment> comment = book.Comments.ToList();
            
            if (comment != null)
            {
                //Сорртируем крментарри по времени
                comment.OrderBy(c=>c.Id);
                book.Comments = comment;
            }

            return View(book);

        }

        //Post-контроллер.Добавление комментария
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel comment)
        {
            Book book = await db.Books.FirstOrDefaultAsync(p => p.Id == comment.BookId);
            Comment Comment = new Comment { BookId = comment.BookId, NameUser = User.Identity.Name, CommentString = comment.CommentString, Book=book };
            db.Comments.Add(Comment);
            book.Comments.Add(Comment);
            db.Books.Update(book);
            await db.SaveChangesAsync();
            return RedirectToAction("GetThisBook", new { id = comment.BookId });
        }
        //Post-контроллер.Добавление оценки
        public IActionResult AddEvaluation(EvaluationViewModel evaluation)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == evaluation.BookId);
            Evaluation Evaluation = db.Evaluations.FirstOrDefault(p => p.BookId == evaluation.BookId);
            if (Evaluation.Average == 0)
            {
                Evaluation.Average =evaluation.Score;
            }
            else
            {
                Evaluation.Average = (byte)((Evaluation.Average + evaluation.Score) / 2);
            }
            Evaluation.Users.Add(evaluation.user);
            Evaluation.Book = book;
            book.Evaluation = Evaluation;
            db.Evaluations.Update(Evaluation);
            db.Books.Update(book);
            db.SaveChanges(); 
            return RedirectToAction("GetThisBook", new { id = evaluation.BookId });
        }
        [Authorize(Roles = "librarian")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            EditBookViewModel editBookViewModel = new EditBookViewModel { Id=book.Id,Title = book.Title, Authtor = book.Authtor, Year = book.Year, Language = book.Language, Genre = book.Genre, Publisher=book.Publisher, Description = book.Description, Image = book.Image };
            return View(editBookViewModel);
        }

        //Post-контроллер. Редактирование информации о книг
        [HttpPost]
        [Authorize(Roles = "librarian")]
        public IActionResult Edit(EditBookViewModel edit)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == edit.Id);
            book.Title = edit.Title;
            book.Authtor = edit.Authtor;
            book.Year = edit.Year;
            book.Language = edit.Language;
            book.Genre = edit.Genre;
            book.Publisher = edit.Publisher;
            book.Description = edit.Description;
            db.Books.Update(book);
            db.SaveChanges();
            return RedirectToAction("GetThisBook", new { id = edit.Id });
        }
       
    }
}
