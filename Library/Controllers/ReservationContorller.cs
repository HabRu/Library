using Library.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Library.Services.EmailServices;
using Library.Services.ReservationControlServices;

namespace Library.Controllers
{

    public class ReservationController : Controller
    {
        ApplicationContext db;
        private readonly IReservationControlService reservationControl;

        public ReservationController(ApplicationContext applicationContext, IReservationControlService reservationControl)
        {
            db = applicationContext;
            this.reservationControl = reservationControl;
        }

        //Get-контроллер. Отказ от резервации
        [Authorize]
        public async Task<IActionResult> Refuse(int? id)
        {
            #region Старая реализация
            //Reservation reservation = db.Reservations.Include(r => r.User).FirstOrDefault(p => p.Id == id);
            //User user = reservation.User;
            //Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            ////Получакм список подписок
            //List<Tracking> trackings = db.Trackings.Include(t => t.User).Where(t => t.BookId == book.Id).ToList();
            ////Отправка сообщения подписчикам, что книга доступна для бронирования
            //foreach (var track in trackings)
            //{
            //    await emailService.SendEmailAsync(track.User.Email, "Книга доступна для бронирования", message.GetMessage(track.User.UserName, book.Title)); ;
            //}
            ////Удаляем подписчиков
            //db.Trackings.RemoveRange(trackings);
            ////Проверка того,что пользователь авторизован
            //if (User.IsInRole("library") || User.Identity.Name == user.Email)
            //{
            //    //Удаление резирвации
            //    book.Status = Status.Available;
            //    db.Reservations.Remove(reservation);
            //    user.ReservUser.Remove(reservation);
            //    //Сохранение изменений
            //    await db.SaveChangesAsync();
            //    if (User.IsInRole("librarian"))
            //    {
            //        return RedirectToAction("ListReserv");
            //    }
            //    else
            //    {
            //        return RedirectToAction("MyPage", "UserPage");

            //    }
            //}
            //return RedirectToAction("ListReserv"); 
            #endregion
            return await reservationControl.Refuse(id, this);
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
            #region Старая реализация
            //Reservation reservation = db.Reservations.FirstOrDefault(p => p.Id == id);
            //Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            //book.Status = Status.Available;
            //reservation.State = ReserveState.Passed;
            //reservation.DataSend = System.DateTime.Now;
            //db.SaveChanges(); 
            #endregion
            await reservationControl.Accept(id, this);
            return RedirectToAction("ListReserv");
        }
        // Создаем резервацию
        [Authorize]
        public async Task<IActionResult> CreateReserv(int? id)
        {
            #region Старая реализация
            //Book book = db.Books.FirstOrDefault(p => p.Id == id);
            //if (User.Identity.IsAuthenticated)
            //{
            //    User user = db.Users.FirstOrDefault(p => p.Id == _userManager.GetUserId(User));
            //    Reservation reservation = new Reservation { BookIdentificator = book.Id, UserId = user.Id, UserName = user.UserName, User = user };
            //    reservation.State = ReserveState.Booked;
            //    reservation.DataBooking = System.DateTime.Now;
            //    db.Reservations.AddAsync(reservation);
            //    book.Status = Status.Booked;
            //    user.ReservUser.Add(reservation);
            //    db.Books.Update(book);
            //    db.SaveChanges();
            //} 
            #endregion
            await reservationControl.CreateReserv(id, this);
            return RedirectToAction("ListBook", "Book");
        }
    }
}
