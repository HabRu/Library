using AutoMapper;
using Library.Models;
using Library.Services.BookContorlServices.BookFilters;
using Library.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using BusinessLayer.InrefacesRepository;

namespace Library.Services.BookContorlServices
{
    public class EFBooksRepository : IBooksRepository
    {
        private readonly IRepository<Book> bookRep;

        private readonly IRepository<Comment> commRep;

        private readonly IRepository<Evaluation> evalRep;

        private readonly IMapper mapper;

        private readonly IConfiguration configuration;

        public EFBooksRepository(IRepository<Book> bookRep, IRepository<Comment> commRep, IRepository<Evaluation> evalRep, IMapper mapper, IConfiguration configuration)
        {
            this.bookRep = bookRep;
            this.commRep = commRep;
            this.evalRep = evalRep;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task AddBook(AddBookViewModel model, string pathWeb)
        {
            //Создаем переменную книги
            var book = mapper.Map<Book>(model);

            //Если изображение не null,
            if (model.Image != null)
            {
                var path = configuration.GetValue<string>("ImagePath") + model.Image.FileName;
                var contentPath = pathWeb + path;

                if (!File.Exists(contentPath))
                {
                    using var fileStream = new FileStream(contentPath, FileMode.Create);
                    await model.Image.CopyToAsync(fileStream);
                }

                book.Image = path;
            }

            //Добавляем книгу в бд
            await bookRep.CreateAsync(book);
        }

        public async Task AddComment(CommentViewModel comment, string NameUser)
        {
            var book = await bookRep.GetByIdAsync(comment.BookId);
            var Comment = mapper.Map<Comment>(comment);
            Comment.Book = book;
            Comment.NameUser = NameUser;
            await commRep.CreateAsync(Comment);
            book.Comments.Add(Comment);
            await bookRep.UpdateAsync(book);
        }


        public async Task AddEvaluation(EvaluationViewModel evaluation)
        {
            var book = await bookRep.GetByIdAsync(evaluation.BookId);
            var Evaluation = await evalRep.GetByIdAsync(evaluation.BookId);
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
            await evalRep.UpdateAsync(Evaluation);
            await bookRep.UpdateAsync(book);
        }

        public void DeleteBook(int? id)
        {
            bookRep.Delete(id);
        }

        public async void Edit(EditBookViewModel edit)
        {
            var book = await bookRep.GetByIdAsync(edit.Id);
            book.Title = edit.Title;
            book.Authtor = edit.Authtor;
            book.Year = edit.Year;
            book.Language = edit.Language;
            book.Genre = edit.Genre;
            book.Publisher = edit.Publisher;
            book.Description = edit.Description;
            await bookRep.UpdateAsync(book);
        }

        public async Task<EditBookViewModel> Edit(int? id)
        {
            var book = await bookRep.GetByIdAsync(id);
            var editBookViewModel = mapper.Map<EditBookViewModel>(book);
            return editBookViewModel;
        }

        public async Task<BookViewModel> GetThisBook(int? id)
        {
            var book = await bookRep.Include(b => b.Comments, b => b.Evaluation).FirstOrDefaultAsync(b => b.Id == id);
            if (book.Evaluation.Users == null)
            {
                book.Evaluation.Users = new List<string>();
            }
            var comment = book.Comments.ToList();

            if (comment != null)
            {
                //Сорртируем крментарри по времени
                comment.OrderBy(c => c.Id);
                book.Comments = comment;
            }
            return mapper.Map<BookViewModel>(book);
        }

        public AllListBookViewModel ListBook(BookFilterModel model)
        {
            var Books = mapper.ProjectTo<BookViewModel>(bookRep.GetAll());
            //Фильтрация книг
            Books = Books.WhereComplex(model);
            //Сортировка книг
            Books = Books.OrderByComplex(model.SortOrder);

            var count = Books.Count();
            var items = Books.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToList();

            // формируем модель представления
            var viewModel = new AllListBookViewModel
            {
                PageViewModel = new PageViewModel(count, model.Page, model.PageSize),
                SortViewModel = new SortViewModel(model.SortOrder),
                FilterViewModel = new FilterViewModel(model.Title, model.Language, model.Authtor, model.Genre, model.Publisher),
                Books = items.ToList()
            };
            return viewModel;
        }
    }
}
