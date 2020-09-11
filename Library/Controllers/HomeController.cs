using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.ViewModels;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;

        public HomeController(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }

        //Контроллер начальной страницы
        public IActionResult Index()
        {
            List<Book> books = db.Books.ToList();
            //Возвращает новые книги
            List<Book> NewBooks = books.TakeLast(2).ToList();
            //Возвращает популярные книги
            List<Book> TopBooks = books.OrderBy(p => p.Evaluation.Average).TakeLast(3).ToList();
            IndexViewModel indexViewModel = new IndexViewModel { NewBooks = NewBooks, TopBooks = TopBooks };
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
