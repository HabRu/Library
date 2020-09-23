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
        private readonly ApplicationContext db;

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

            await reservationControl.DeleteReserv(id, User.Identity.Name, User.IsInRole(RolesConfig.LIBRARIAN));

            if (User.IsInRole(RolesConfig.LIBRARIAN))
            {
                return RedirectToAction("ListReserv");
            }
            return RedirectToAction("MyPage", "UserPage");
        }

        //Список всех резераций
        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        public async Task<IActionResult> ListReserv()
        {
            var reservations = db.Reservations;
            return View(await reservations.AsNoTracking().ToListAsync());

        }

        //Контроллер для сдачи книги
        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        public async Task<IActionResult> Accept(int? id)
        {
            var user = await userManager.GetUserAsync(User);
            user.ReservUser.Add(await reservationControl.CreateReserv(id, user.Id, user.NameUser));
            db.SaveChanges();
            return RedirectToAction("ListReserv");
        }

        // Создаем резервацию
        [Authorize]
        public async Task<IActionResult> CreateReserv(int? id)
        {
            var user = await userManager.GetUserAsync(User);
            user.ReservUser.Add(await reservationControl.CreateReserv(id, user.Id, user.NameUser));
            db.SaveChanges();
            return RedirectToAction("ListBook", "Book");
        }
    }
}
