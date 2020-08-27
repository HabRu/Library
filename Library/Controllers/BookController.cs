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
        [Authorize(Roles = "librarian")]
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookViewModel model)
        {
            Book book = new Book { Id = model.Id, Title = model.Title, Language = model.Language, Authtor = model.Authtor, Year = model.Year,Publisher=model.Publisher, Genre = model.Genre,Status=Status.Естьвналичии };
            if (model.Image != null)
            {
                byte[] imageData = null;
                using(var binaryReader=new BinaryReader(model.Image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.Image.Length);
                }
                book.Image = imageData;
            }
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index","Home");

        }
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
        [AllowAnonymous]
        [Authorize]
       public async Task<IActionResult> ListBook(string title,string language,string author,string genre,string publisher, int page=1,SortState sortOrder=SortState.NameAsc) 
        {
            int pageSize = 7;

            IQueryable<Book> Books = db.Books.Include(b=>b.TrackingList).ThenInclude(t=>t.User);
            if (title != null)
            {
                Books = Books.Where(p => p.Title == title);
            }
            if (language != null)
            {
                Books = Books.Where(p => p.Language == language);
            }
            if (author != null)
            {
                Books = Books.Where(p => p.Authtor == author);
            }
            if (genre != null)
            {
                Books = Books.Where(p => p.Genre == genre);
            }
            if (publisher!=null)
            {
                Books = Books.Where(p => p.Publisher == publisher);
            }
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
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetThisBook(int? id)
        {
            Book book =await db.Books.Include(b=>b.Comments).Include(b=>b.Evaluation).FirstOrDefaultAsync(p => p.Id == id);
            List<Comment> comment = book.Comments.ToList();
            Evaluation evaluation = book.Evaluation;
            comment.Reverse();
            book.Evaluation = evaluation;
            if (comment != null)
            {
                book.Comments = comment;
            }

            return View(book);

        }
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
