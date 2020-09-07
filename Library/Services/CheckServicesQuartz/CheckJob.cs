using Library.Models;
using Library.Services.EmailServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.CheckServicesQuartz
{
    public class CheckJob : IJob
    {
        private readonly EmailService emailService;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly MessageForm message;
        private readonly Settings settings;

        public CheckJob(EmailService emailService, IServiceScopeFactory serviceScopeFactory, IOptions<Settings> settings, MessageForm message)
        {
            this.settings = settings.Value;
            this.emailService = emailService;
            this.serviceScopeFactory = serviceScopeFactory;
            this.message = message;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await CheckReservs();

        }

        public async Task CheckReservs()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                if (db.Reservations.Count() > 0)
                {
                    foreach (Reservation reservation in db.Reservations)
                    {
                        await CheckDateAsync(reservation);
                    }
                }
            }
        }

        //Метод для проверки не истек ли срок бронирования
        public async Task CheckDateAsync(Reservation reservation)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                if (reservation.State != ReserveState.Passed)
                {
                    //Если прошло больше settings.TimeReservation  дней тода удаляем бронирование и отправляем уведомления, тем кто отслеживает эту книгу
                    if (DateTime.Now.Subtract(reservation.DataBooking).Days > settings.TimeReservation)
                    {
                        List<Tracking> trackings = db.Trackings.Include(t => t.User).Include(t => t.Book).Where(t => t.BookId == reservation.BookIdentificator).ToList();
                        if (trackings.Count > 0)
                        {
                            foreach (var trac in trackings)
                            {
                                trac.Book.Status = Status.Available;
                                await emailService.SendEmailAsync(trac.User.Email, "Книга доступна для бронирования", message.GetMessage(trac.User.UserName, trac.Book.Title));
                            }

                        }
                        db.Reservations.Remove(reservation);
                        db.Trackings.RemoveRange(trackings);
                        db.SaveChanges();
                    }
                }


            }
        }
    }
}
