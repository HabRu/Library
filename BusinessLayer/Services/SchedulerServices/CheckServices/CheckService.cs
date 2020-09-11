using Library.Models;
using Library.Services.CheckServicesQuartz;
using Library.Services.EmailServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Services.CheckServices
{
    //Сервис для проверки не истек ли срок бронирования
    public class CheckService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<EmailScheduler> _emailSchedules;

        public CheckService(
            ISchedulerFactory schedulerFactory,
            EmailService emailService,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<Settings> settings,
            IJobFactory jobFactory,
            IEnumerable<EmailScheduler> emailSchedulers)
        {
            _schedulerFactory = schedulerFactory;
            _emailSchedules = emailSchedulers;
            _jobFactory = jobFactory;
        }

        public IScheduler Scheduler { get; set; }

        //Запуск сервиса
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var emailSchedule in _emailSchedules)
            {
                var job = CreateJob(emailSchedule);
                var trigger = CreateTrigger(emailSchedule);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(EmailScheduler schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(EmailScheduler schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }
    }
}
