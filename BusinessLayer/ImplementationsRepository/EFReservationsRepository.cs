using Library.Models;
using Library.Services.EmailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.ReservationControlServices
{
    public class EFReservationsRepository : IReservationRepository
    {
        private readonly ApplicationContext db;

        private readonly EmailService emailService;

        private readonly MessageForm message;

        private readonly UserManager<User> _userManager;

        public EFReservationsRepository(ApplicationContext db, EmailService emailService,
                                        MessageForm message, UserManager<User> _userManager)
        {
            this.db = db;
            this.emailService = emailService;
            this.message = message;
            this._userManager = _userManager;
        }

        public async Task AcceptReserv(int? id)
        {
            Reservation reservation = db.Reservations.FirstOrDefault(p => p.Id == id);
            Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
            book.Status = Status.Available;
            reservation.State = ReserveState.Passed;
            reservation.DataSend = System.DateTime.Now;
            await db.SaveChangesAsync();

        }

        public async Task CreateReserv(int? id, string userId)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);

            User user = db.Users.FirstOrDefault(p => p.Id == userId);
            Reservation reservation = new Reservation { BookIdentificator = book.Id, UserId = user.Id, UserName = user.UserName, User = user };
            reservation.State = ReserveState.Booked;
            reservation.DataBooking = System.DateTime.Now;
            await db.Reservations.AddAsync(reservation);
            book.Status = Status.Booked;
            user.ReservUser.Add(reservation);
            db.Books.Update(book);
            await db.SaveChangesAsync();
        }

        public async Task DeleteReserv(int? id, string name, bool hasAccess)
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
            if (hasAccess || name == user.Email)
            {
                //Удаление резирвации
                book.Status = Status.Available;
                db.Reservations.Remove(reservation);
                user.ReservUser.Remove(reservation);
                //Сохранение изменений
                await db.SaveChangesAsync();

            }
        }
    }
}
