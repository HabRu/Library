using Library.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Library.Services.EmailServices;

namespace Library.Controllers
{
  
    public class ReservationController:Controller
    {
        UserManager<User> _userManager;
        private readonly EmailService emailService;
        private readonly MessageForm message;
        ApplicationContext db;


        public ReservationController(ApplicationContext applicationContext,UserManager<User> userManager, EmailService emailService,MessageForm message)
        {
            _userManager = userManager;
            this.emailService = emailService;
            this.message = message;
            db = applicationContext;
        }

        //Get-контроллер. Отказ от резервации
        [Authorize]
        public async Task<IActionResult> Refuse(int? id)
        {

            Reservation reservation = db.Reservations.Include(r=>r.User).FirstOrDefault(p => p.Id == id);
            User user = reservation.User;
            Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            //Получакм список подписок
            List<Tracking> trackings = db.Trackings.Include(t => t.User).Where(t => t.BookId == book.Id ).ToList();
            //Отправка сообщения подписчикам, что книга доступна для бронирования
            foreach (var track in trackings)
            {
                await emailService.SendEmailAsync(track.User.Email, "Книга доступна для бронирования", message.GetMessage(track.User.UserName, book.Title)); ;
            }
            //Удаляем подписчиков
            db.Trackings.RemoveRange(trackings);
            //Проверка того,что пользователь авторизован
            if(User.IsInRole("library") || User.Identity.Name == user.Email)
            {
                //Удаление резирвации
                book.Status = Status.Естьвналичии;
                db.Reservations.Remove(reservation);
                user.ReservUser.Remove(reservation);
                //Сохранение изменений
                await db.SaveChangesAsync();
                if (User.IsInRole("librarian"))
                {
                    return RedirectToAction("ListReserv");
                }
                else
                {
                    return RedirectToAction("MyPage", "UserPage");

                }
            }
            return RedirectToAction("ListReserv");
        }
        //Список всех резераций
        [Authorize(Roles = "librarian")]
        public async Task<IActionResult> ListReserv()
        {
            IQueryable<Reservation> reservations = db.Reservations;
            return View(await reservations.AsNoTracking().ToListAsync());

        }

        //Контроллер для сдачи книги
        [Authorize(Roles = "librarian")]
        public IActionResult Accept(int? id)
        {
            Reservation reservation = db.Reservations.FirstOrDefault(p=>p.Id==id);
            Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            book.Status = Status.Сдан;
            reservation.State = ReserveState.Сдан;
            reservation.DataSend = System.DateTime.Now;
            db.SaveChanges();
            
            return RedirectToAction("ListReserv");
        }
        // Создаем резервацию
        [Authorize]
        public IActionResult CreateReserv(int? id)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            if (User.Identity.IsAuthenticated)
            {
                User user =db.Users.FirstOrDefault(p=>p.Id==_userManager.GetUserId(User));
                Reservation reservation = new Reservation { BookIdentificator = book.Id, UserId = user.Id, UserName = user.UserName,User=user };
                reservation.State = ReserveState.Забронирован;
                reservation.DataBooking = System.DateTime.Now;
                db.Reservations.AddAsync(reservation);
                book.Status = Status.Забронирован;
                user.ReservUser.Add(reservation);
                db.Books.Update(book);
                db.SaveChanges();
            }
           return  RedirectToAction("ListBook", "Book");
        }
    }
}
