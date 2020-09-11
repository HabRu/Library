using Library.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Library.Services.ReservationControlServices;

namespace Library.Controllers
{
    public class ReservationController : Controller
    {
        ApplicationContext db;

        private readonly IReservationRepository reservationControl;

        private readonly UserManager<User> userManager;

        public ReservationController(ApplicationContext applicationContext, IReservationRepository reservationControl, UserManager<User> userManager)
        {
            db = applicationContext;
            this.reservationControl = reservationControl;
            this.userManager = userManager;
        }

        //Get-контроллер. Отказ от резервации
        [Authorize]
        public async Task<IActionResult> Refuse(int? id)
        {

            await reservationControl.DeleteReserv(id, User.Identity.Name, User.IsInRole(RolesConfig.librarian));

            if (User.IsInRole("librarian"))
            {
                return RedirectToAction("ListReserv");
            }
            return RedirectToAction("MyPage", "UserPage");
        }

        //Список всех резераций
        [Authorize(Roles = RolesConfig.librarian)]
        public async Task<IActionResult> ListReserv()
        {
            IQueryable<Reservation> reservations = db.Reservations;
            return View(await reservations.AsNoTracking().ToListAsync());

        }

        //Контроллер для сдачи книги
        [Authorize(Roles = RolesConfig.librarian)]
        public async Task<IActionResult> Accept(int? id)
        {
            await reservationControl.CreateReserv(id, userManager.GetUserId(User));
            return RedirectToAction("ListReserv");
        }

        // Создаем резервацию
        [Authorize]
        public async Task<IActionResult> CreateReserv(int? id)
        {
            await reservationControl.CreateReserv(id, userManager.GetUserId(User));
            return RedirectToAction("ListBook", "Book");
        }
    }
}
