using AutoMapper;
using Library.Models;
using Library.Tag;
using Library.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.BookContorlServices
{
    public class BookControlService : IBookControlService
    {
        private readonly ApplicationContext db;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment hostEnvironment;

        public BookControlService(ApplicationContext db, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.mapper = mapper;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task AddBook(AddBookViewModel model)
        {
            //Создаем переменную книги
            Book book = mapper.Map<Book>(model);

            //Если изображение не null,
            if (model.Image != null)
            {

                var path = "/images/" + model.Image.FileName;
                var contentPath = hostEnvironment.WebRootPath + path;

                if (!File.Exists(contentPath))
                {
                    using (var fileStream = new FileStream(contentPath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(fileStream);
                    }
                }

                book.Image = path;
            }

            //Добавляем книгу в бд
            db.Books.Add(book);
            //Сохраняем изменения
            await db.SaveChangesAsync();
        }

        public async Task AddComment(CommentViewModel comment, string NameUser)
        {
            Book book = await db.Books.FirstOrDefaultAsync(p => p.Id == comment.BookId);
            Comment Comment = mapper.Map<Comment>(comment);
            Comment.Book = book;
            Comment.NameUser = NameUser;
            db.Comments.Add(Comment);
            book.Comments.Add(Comment);
            db.Books.Update(book);
            await db.SaveChangesAsync();
        }


        public async Task AddEvaluation(EvaluationViewModel evaluation)
        {
            Book book = await db.Books.FirstOrDefaultAsync(p => p.Id == evaluation.BookId);
            Evaluation Evaluation = await db.Evaluations.FirstOrDefaultAsync(p => p.BookId == evaluation.BookId);
            if (Evaluation.Average == 0)
            {
                Evaluation.Average = evaluation.Score;
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
            await db.SaveChangesAsync();

        }

        public async Task DeleteBook(int? id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(p => p.Id == id);
            if (book != null)
            {
                db.Books.Remove(book);
                await db.SaveChangesAsync();
            }
        }

        public void Edit(EditBookViewModel edit)
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
        }

        public EditBookViewModel Edit(int? id)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            EditBookViewModel editBookViewModel =mapper.Map<EditBookViewModel>(book);
            return editBookViewModel;
        }

        public async Task<BookViewModel> GetThisBook(int? id)
        {
            Book book = await db.Books.Include(b => b.Comments).Include(b => b.Evaluation).Include(b => b.Evaluation).FirstOrDefaultAsync(p => p.Id == id);
            List<Comment> comment = book.Comments.ToList();

            if (comment != null)
            {
                //Сорртируем крментарри по времени
                comment.OrderBy(c => c.Id);
                book.Comments = comment;
            }
            return mapper.Map<BookViewModel>(book);
        }

        public async Task<AllListBookViewModel> ListBook(BookFilterModel model)
        {

            IQueryable<Book> Books = db.Books.Include(b => b.TrackingList).ThenInclude(t => t.User);

            //BEGIN: Конвейр фильтраиции
            if (model.Title != null)
            {
                Books = Books.Where(p => p.Title.StartsWith(model.Title));
            }
            if (model.Language != null)
            {
                Books = Books.Where(p => p.Language.StartsWith(model.Language));
            }
            if (model.Authtor != null)
            {
                Books = Books.Where(p => p.Authtor.StartsWith(model.Authtor));
            }
            if (model.Genre != null)
            {
                Books = Books.Where(p => p.Genre.StartsWith(model.Genre));
            }
            if (model.Publisher != null)
            {
                Books = Books.Where(p => p.Publisher.StartsWith(model.Publisher));
            }
            //END: Конвейр фильтраиции

            //Сортировка книг
            switch (model.SortOrder)
            {
                case SortState.NameDesc:
                    Books = Books.OrderByDescending(s => s.Title);
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
            var items = await Books.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToListAsync();

            // формируем модель представления
            AllListBookViewModel viewModel = new AllListBookViewModel
            {
                PageViewModel = new PageViewModel(count, model.Page, model.PageSize),
                SortViewModel = new SortViewModel(model.SortOrder),
                FilterViewModel = new FilterViewModel(model.Title, model.Language, model.Authtor, model.Genre, model.Publisher),
                Books = mapper.Map<IEnumerable<BookViewModel>>(items)
            };
            return viewModel;
        }
    }
}
