using Library.Models;
using Library.Services.EmailServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Services.CheckServices
{
    public class CheckService:IHostedService,IDisposable
    {
        private readonly EmailService emailService;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly Settings settings;
        private readonly MessageForm message;
        private Timer timer;

        public CheckService(EmailService emailService,IServiceScopeFactory serviceScopeFactory,Settings settings,MessageForm message)
        {
            this.emailService = emailService;
            this.serviceScopeFactory = serviceScopeFactory;
            this.settings = settings;
            this.message = message;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var interval = settings.RunInterval;

            timer = new Timer((e) => CheckReservs(),
                                null,
                                TimeSpan.Zero,
                                TimeSpan.FromDays(interval));
            return Task.CompletedTask;
        }

        public void CheckReservs()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                if (db.Reservations.Count() > 0)
                {
                    foreach (Reservation reservation in db.Reservations)
                    {
                        CheckDate(reservation);
                    }
                }
            }
        }

        public async void CheckDate(Reservation reservation)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                if (reservation.State != ReserveState.Сдан)
                {
                    if (DateTime.Now.Subtract(reservation.DataBooking).Days > settings.TimeReservation)
                    {
                        List<Tracking> trackings = db.Trackings.Include(t => t.User).Include(t=>t.Book).Where(t => t.BookId == reservation.BookIdentificator).ToList();
                        if (trackings.Count > 0)
                        {
                            foreach (var trac in trackings)
                            {
                                trac.Book.Status = Status.Естьвналичии;
                                await emailService.SendEmailAsync(trac.User.Email, "Книга доступна для бронирования", message.GetMessage(trac.User.UserName,trac.Book.Title));
                            }

                        }
                        db.Reservations.Remove(reservation);
                        db.Trackings.RemoveRange(trackings);
                        db.SaveChanges();
                    }
                }
           
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }



    }
}
