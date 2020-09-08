using Library.Models;
using Library.Services.EmailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.ReservationControlServices
{
    public class ReservationControlService : IReservationControlService
    {
        private readonly ApplicationContext db;
        private readonly EmailService emailService;
        private readonly MessageForm message;
        private readonly UserManager<User> _userManager;

        public ReservationControlService(ApplicationContext db, EmailService emailService,
                                        MessageForm message, UserManager<User> _userManager)
        {
            this.db = db;
            this.emailService = emailService;
            this.message = message;
            this._userManager = _userManager;
        }
        public async Task Accept(int? id, Controller controller)
        {
            Reservation reservation = db.Reservations.FirstOrDefault(p => p.Id == id);
            Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            book.Status = Status.Available;
            reservation.State = ReserveState.Passed;
            reservation.DataSend = System.DateTime.Now;
            await db.SaveChangesAsync();
            
        }

        public async Task CreateReserv(int? id, Controller controller)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            if (controller.User.Identity.IsAuthenticated)
            {
                User user = db.Users.FirstOrDefault(p => p.Id == _userManager.GetUserId(controller.User));
                Reservation reservation = new Reservation { BookIdentificator = book.Id, UserId = user.Id, UserName = user.UserName, User = user };
                reservation.State = ReserveState.Booked;
                reservation.DataBooking = System.DateTime.Now;
                await db.Reservations.AddAsync(reservation);
                book.Status = Status.Booked;
                user.ReservUser.Add(reservation);
                db.Books.Update(book);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> Refuse(int? id, Controller controller)
        {
            Reservation reservation = db.Reservations.Include(r => r.User).FirstOrDefault(p => p.Id == id);
            User user = reservation.User;
            Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            //Получакм список подписок
            List<Tracking> trackings = db.Trackings.Include(t => t.User).Where(t => t.BookId == book.Id).ToList();
            //Отправка сообщения подписчикам, что книга доступна для бронирования
            foreach (var track in trackings)
            {
                await emailService.SendEmailAsync(track.User.Email, "Книга доступна для бронирования", message.GetMessage(track.User.UserName, book.Title)); ;
            }
            //Удаляем подписчиков
            db.Trackings.RemoveRange(trackings);
            //Проверка того,что пользователь авторизован
            if (controller.User.IsInRole("library") || controller.User.Identity.Name == user.Email)
            {
                //Удаление резирвации
                book.Status = Status.Available;
                db.Reservations.Remove(reservation);
                user.ReservUser.Remove(reservation);
                //Сохранение изменений
                await db.SaveChangesAsync();
                if (controller.User.IsInRole("librarian"))
                {
                    return controller.RedirectToAction("ListReserv");
                }
                else
                {
                    return controller.RedirectToAction("MyPage", "UserPage");

                }
            }
            return controller.RedirectToAction("ListReserv");
        }
    }
}
