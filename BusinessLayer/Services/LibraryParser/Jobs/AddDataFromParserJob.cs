using AutoMapper;
using BusinessLayer.InrefacesRepository;
using BusinessLayer.Services.LibraryParser.Model;
using BusinessLayer.Services.LibraryParser.ParserInterfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Services.LibraryParser.Jobs
{
    public class AddDataFromParserJob : IJob
    {
        private readonly IParser<IEnumerable<string>> parserLists;
        private readonly IParser<BookParserModel> parserBook;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IRepository<Book> bookRep;
        private readonly ParserSettings options;
        private readonly IList<Book> books;

        public AddDataFromParserJob(IParser<IEnumerable<string>> parserLists,
                                    IParser<BookParserModel> parserBook,
                                    IOptions<ParserSettings> options,
                                    IServiceScopeFactory serviceScopeFactory,
                                    IConfiguration configuration,
                                    IMapper mapper,
                                    IRepository<Book> bookRep)
        {
            this.parserLists = parserLists;
            this.parserBook = parserBook;
            this.serviceScopeFactory = serviceScopeFactory;
            this.configuration = configuration;
            this.mapper = mapper;
            this.bookRep = bookRep;
            this.options = options.Value;
            books = new List<Book>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            for (var i = 2; i < options.Count; i++)
            {
                var href = options.HrefBooks + i.ToString();
                var listBookHref = (IEnumerable<string>)(await parserLists.ParseAsync(href));
                foreach (var bookHref in listBookHref)
                {
                    if (string.IsNullOrWhiteSpace(bookHref))
                        continue;
                    var hrefBook = options.Host + bookHref;
                    var bookModel = await parserBook.ParseAsync(hrefBook);
                    if (bookModel != null)
                    {
                        var book = mapper.Map<Book>(bookModel);
                        if (book != null)
                        {
                            await AddBookAsync(book);
                        }
                    }
                }
            }
           
        }

        public async Task AddBookAsync(Book bookModel)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var hasBook = db.Set<Book>().FirstOrDefault(b => b.Title.Contains(bookModel.Title));

                if (hasBook == null)
                {
                   db.Set<Book>().Add(bookModel);
                   await db.SaveChangesAsync();
                }
            }

        }
    }
}
