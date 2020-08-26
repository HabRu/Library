using Library.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Library.Controllers
{
  
    public class ReservationController:Controller
    {
        UserManager<User> _userManager;
        ApplicationContext db;
        public ReservationController(ApplicationContext applicationContext,UserManager<User> userManager)
        {
            _userManager = userManager;
            db = applicationContext;
        }
        [Authorize]
        public async Task<IActionResult> Refuse(int? id)
        {
            Reservation reservation = db.Reservations.Include(r=>r.User).FirstOrDefault(p => p.Id == id);
            User user = reservation.User;
            Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            if (User.IsInRole("librarian") || User.Identity.Name == user.Email)
            {
                book.Status = Status.Естьвналичии;
                db.Reservations.Remove(reservation);
                user.ReservUser.Remove(reservation);
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
        [Authorize(Roles = "librarian")]
        public async Task<IActionResult> ListReserv()
        {
            IQueryable<Reservation> reservations = db.Reservations;
            return View(await reservations.AsNoTracking().ToListAsync());

        }
        [Authorize(Roles = "librarian")]
        public IActionResult Accept(int? id)
        {
            Reservation reservation = db.Reservations.FirstOrDefault(p=>p.Id==id);
            Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            book.Status = Status.Сдан;
            reservation.State = ReserveState.Сдан;
            reservation.DataSend = System.DateTime.Now.ToString("Год:yyyy Месяц:MM День:dd Час:hh");
            db.SaveChanges();
            
            return RedirectToAction("ListReserv");
        }
        [Authorize]
        public IActionResult CreateReserv(int? id)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            if (User.Identity.IsAuthenticated)
            {
                User user =db.Users.FirstOrDefault(p=>p.Id==_userManager.GetUserId(User));
                Reservation reservation = new Reservation { BookIdentificator = book.Id, UserId = user.Id, UserName = user.UserName,User=user };
                reservation.State = ReserveState.Забронирован;
                reservation.DataBooking = System.DateTime.Now.ToString("Год:yyyy Месяц:MM День:dd Час:hh Минуты:mm");
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
