﻿using AutoMapper;
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
        private readonly IRepository db;

        private readonly IMapper mapper;

        private readonly IConfiguration configuration;

        public EFBooksRepository(IRepository db, IMapper mapper, IConfiguration configuration)
        {
            this.db = db;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task AddBook(AddBookViewModel model, string pathWeb)
        {
            //Создаем переменную книги
            Book book = mapper.Map<Book>(model);

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
            db.Books.Add(book);
            //Сохраняем изменения
            await db.DbContext.SaveChangesAsync();
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
            await db.DbContext.SaveChangesAsync();
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
            await db.DbContext.SaveChangesAsync();

        }

        public async Task DeleteBook(int? id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(p => p.Id == id);
            if (book != null)
            {
                db.Books.Remove(book);
                await db.DbContext.SaveChangesAsync();
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
            db.DbContext.SaveChanges();
        }

        public EditBookViewModel Edit(int? id)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            EditBookViewModel editBookViewModel = mapper.Map<EditBookViewModel>(book);
            return editBookViewModel;
        }

        public async Task<BookViewModel> GetThisBook(int? id)
        {
            Book book = await db.Books.Include(b => b.Comments).Include(b => b.Evaluation).Include(b => b.Evaluation).FirstOrDefaultAsync(p => p.Id == id);
            if (book.Evaluation.Users == null)
            {
                book.Evaluation.Users = new List<string>();
                db.DbContext.SaveChanges();
            }
            List<Comment> comment = book.Comments.ToList();

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
            IQueryable<BookViewModel> Books = mapper.ProjectTo<BookViewModel>(db.Books);
            //Фильтрация книг
            Books = Books.WhereComplex(model);
            //Сортировка книг
            Books = Books.OrderByComplex(model.SortOrder);

            var count = Books.Count();
            var items = Books.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToList();

            // формируем модель представления
            AllListBookViewModel viewModel = new AllListBookViewModel
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
