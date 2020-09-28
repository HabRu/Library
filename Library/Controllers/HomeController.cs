using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BusinessLayer.InrefacesRepository;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Book> bookRep;

        public HomeController(IRepository<Book> bookRep)
        {
            this.bookRep = bookRep;
        }

        //Контроллер начальной страницы
        public async Task<IActionResult> Index()
        {
            var booksQuery = bookRep.GetAll();
            //Возвращает новые книги
            var newBooks = await booksQuery.OrderBy(b => b.Id).Take(3).ToListAsync();
            //Возвращает популярные книги
            var topBooks = await booksQuery.OrderByDescending(p => p.Evaluation.Average)
                .Take(3)
                .ToListAsync();
            var indexViewModel = new IndexViewModel { NewBooks = newBooks, TopBooks = topBooks };
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
