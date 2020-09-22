using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        readonly ApplicationContext db;

        public HomeController(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }

        //Контроллер начальной страницы
        public async Task<IActionResult> Index()
        {
            IQueryable<Book> booksQuery = db.Books;
            //Возвращает новые книги
            IList<Book> newBooks = await booksQuery.OrderBy(b => b.Id).Take(3).ToListAsync();
            //Возвращает популярные книги
            IList<Book> topBooks = await booksQuery.OrderByDescending(p => p.Evaluation.Average)
                .Take(3)
                .ToListAsync();
            IndexViewModel indexViewModel = new IndexViewModel { NewBooks = (List<Book>)newBooks, TopBooks = (List<Book>)topBooks };
            return View(indexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
