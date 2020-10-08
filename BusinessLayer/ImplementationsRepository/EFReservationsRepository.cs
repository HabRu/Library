using BusinessLayer.ImplementationsRepository;
using BusinessLayer.InrefacesRepository;
using Library.Models;
using Library.Services.EmailServices;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.ReservationControlServices
{
    public class EFReservationsRepository : IReservationRepository
    {
        private readonly IRepository<Reservation> resRep;

        private readonly IRepository<Book> bookRep;

        private readonly IRepository<Tracking> trackRep;

        private readonly EmailService emailService;

        private readonly MessageForm message;

        public EFReservationsRepository(IRepository<Reservation> resRep, IRepository<Book> bookRep, IRepository<Tracking> trackRep, EmailService emailService,
                                        MessageForm message)
        {
            this.resRep = resRep;
            this.bookRep = bookRep;
            this.trackRep = trackRep;
            this.emailService = emailService;
            this.message = message;
        }

        public async Task AcceptReserv(int? id)
        {
            var reservation = await resRep.GetByIdAsync(id);
            var book = await bookRep.GetByIdAsync(reservation.BookIdentificator);
            book.Status = Status.Available;
            await bookRep.UpdateAsync(book);
            reservation.State = ReserveState.Passed;
            reservation.DataSend = System.DateTime.Now;
            await resRep.UpdateAsync(reservation);
        }

        public async Task<Reservation> CreateReserv(int? id, string userId, string userName)
        {
            var book = await bookRep.GetByIdAsync(id);
            var reservation = new Reservation { BookIdentificator = book.Id, UserId = userId, UserName = userName };
            reservation.State = ReserveState.Booked;
            reservation.DataBooking = System.DateTime.Now;
            await resRep.CreateAsync(reservation);
            book.Status = Status.Booked;
            await bookRep.UpdateAsync(book);
            return reservation;
        }

        public async Task DeleteReserv(int? id, string name, bool hasAccess)
        {
            var reservation = resRep.Include(r => r.User).FirstOrDefault(p => p.Id == id);
            var user = reservation.User;
            var book = await bookRep.GetByIdAsync(reservation.BookIdentificator);
            //Получакм список подписок
            var trackings = trackRep.Include(t => t.User).Where(t => t.BookId == book.Id).ToList();
            //Отправка сообщения подписчикам, что книга доступна для бронирования
            foreach (var track in trackings)
            {
                await emailService.SendEmailAsync(track.User.Email, "Книга доступна для бронирования", message.GetMessage(track.User.UserName, book.Title)); ;
            }
            //Удаляем подписчиков
            reservation = await resRep.GetByIdAsync(id);
            await trackRep.DeleteRangeAsync(trackings);
            //Проверка того,что пользователь авторизован
            if (hasAccess || name == user.Email)
            {
                //Удаление резирвации
                book.Status = Status.Available;
                reservation.State = ReserveState.Stored;
                //Сохранение изменений
                await resRep.SaveChanges();
                await bookRep.SaveChanges();
            }
        }
    }
}
