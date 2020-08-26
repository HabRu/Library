using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.ViewModels;
using Microsoft.AspNetCore.Cors;

namespace Library.Controllers
{
    [EnableCors("AllowAllOrigin")]
    public class HomeController : Controller
    {
        ApplicationContext db;
        public HomeController(ApplicationContext applicationContext)
        {
            db = applicationContext;
        }
        public IActionResult Index()
        {
            List<Book> books = db.Books.ToList();
            List<Book> NewBooks = books.TakeLast(2).ToList();
            List<Book> TopBooks = books.OrderBy(p => p.Evaluation.Average).Take(3).ToList();
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
