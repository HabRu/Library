using Library.Models;
using Library.Services.EmailServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Services.CheckServices
{
    //Сервис для проверки не истек ли срок бронирования
    public class CheckService : IHostedService, IDisposable
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

        //Запуск сервиса
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Интервал запуска сервиса
            var interval = settings.RunInterval;
            //Запускаем таймер,который будет вызывать метод каждые interval дней
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

        //Метод для проверки не истек ли срок бронирования
        public async void CheckDate(Reservation reservation)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                if (reservation.State != ReserveState.Passed)
                {
                    //Если прошло больше settings.TimeReservation  дней тода удаляем бронирование и отправляем уведомления, тем кто отслеживает эту книгу
                    if (DateTime.Now.Subtract(reservation.DataBooking).Days > settings.TimeReservation)
                    {
                        List<Tracking> trackings = db.Trackings.Include(t => t.User).Include(t=>t.Book).Where(t => t.BookId == reservation.BookIdentificator).ToList();
                        if (trackings.Count > 0)
                        {
                            foreach (var trac in trackings)
                            {
                                trac.Book.Status = Status.Available;
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
