using Library.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
namespace Library
{
    internal interface IScopedProcessingService
    {
        void DoWork();
    }

    internal class ScopedProcessingService : IScopedProcessingService
    {
        private readonly ILogger _logger;
        private ApplicationContext db;
             
        public ScopedProcessingService(ILogger<ScopedProcessingService> logger,ApplicationContext applicationContext)
        {
            db = applicationContext;
            _logger = logger;
        }

        public void DoWork()
        {
            List<Reservation> reservations = db.Reservations.ToList();
            
            foreach (Reservation reservation in reservations)
            {
                int i= reservation.DataBooking.LastIndexOf(":");
                string hour = reservation.DataBooking.Substring(i+1);
                StreamWriter streamWriter = new StreamWriter(@"C:\Users\Ильнар\Desktop\text.txt");
                streamWriter.WriteLine(hour);
                streamWriter.Close();
                if (Math.Abs(Convert.ToInt32(i)- DateTime.Now.Minute) >= 5)
                {
                    User user = db.Users.FirstOrDefault(p => p.Id == reservation.UserId);
                    Book book = db.Books.FirstOrDefault(p => p.Id == reservation.BookIdentificator);
                    book.Status = Status.Естьвналичии;
                    db.Reservations.Remove(reservation);
                    user.ReservUser.Remove(reservation);
                }
            }
             db.SaveChanges();
            _logger.LogInformation("Scoped Processing Service is working.");
        }

    }
    internal class ConsumeScopedServiceHostedService : IHostedService
    {
        private readonly ILogger _logger;

        public ConsumeScopedServiceHostedService(IServiceProvider services,
            ILogger<ConsumeScopedServiceHostedService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is starting.");

            DoWork();

            return Task.CompletedTask;
        }

        private void DoWork()
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();

                scopedProcessingService.DoWork();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            return Task.CompletedTask;
        }
    }
}
