using Library.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Library.Services.ReservationControlServices;
using BusinessLayer.InrefacesRepository;
using BusinessLayer.ViewModels;
using System;
using BusinessLayer.Services.GetReportService;
using NPOI.POIFS.EventFileSystem;

namespace Library.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IRepository<Reservation> reservRep;

        private readonly IReservationRepository reservationControl;

        private readonly UserManager<User> userManager;

        private readonly IGetReportService getReportService;

        public ReservationController(IRepository<Reservation> reservRep, IReservationRepository reservationControl, UserManager<User> userManager, IGetReportService getReportService)
        {
            this.reservRep = reservRep;
            this.reservationControl = reservationControl;
            this.userManager = userManager;
            this.getReportService = getReportService;
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
            var reservations = reservRep.GetAll().Where(r => r.State != ReserveState.Stored);
            return View(await reservations.AsNoTracking().ToListAsync());

        }

        //Контроллер для сдачи книги
        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        public async Task<IActionResult> Accept(int? id)
        {
            await reservationControl.AcceptReserv(id);
            await reservRep.SaveChanges();
            return RedirectToAction("ListReserv");
        }

        // Создаем резервацию
        [Authorize]
        public async Task<IActionResult> CreateReserv(int? id)
        {
            var user = await userManager.GetUserAsync(User);
            user.ReservUser.Add(await reservationControl.CreateReserv(id, user.Id, user.NameUser));
            await reservRep.SaveChanges();
            return RedirectToAction("ListBook", "Book");
        }

        [HttpGet]
        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        public IActionResult GetReport()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RolesConfig.LIBRARIAN)]
        public IActionResult GetReport(GetReportViewModel getReport)
        {
            var stream = getReportService.GetReportExcel(getReport);

            var file_type = "application/vnd.ms-excel";
            var file_name = "Отчет.xlsx";

            return File(stream, file_type, file_name);
        }
    }
}
