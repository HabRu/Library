using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Services.LibraryParser
{
    public class StartParserService : IHostedService
    {
        private readonly ISchedulerFactory schedulerFactory;
        private readonly IJobFactory jobFactory;
        private readonly IEnumerable<ParserScheduler> parserSchedulers;

        public IScheduler Scheduler { get; set; }

        public StartParserService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IEnumerable<ParserScheduler> parserSchedulers)
        {
            this.schedulerFactory = schedulerFactory;
            this.jobFactory = jobFactory;
            this.parserSchedulers = parserSchedulers;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = jobFactory;

            foreach (var parserScheduler in parserSchedulers)
            {
                var job = CreateJob(parserScheduler);
                var trigger = CreateTrigger(parserScheduler);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(ParserScheduler scheduler)
        {
            var jobType = scheduler.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(ParserScheduler scheduler)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{scheduler.JobType.FullName}.trigger")
                .WithCronSchedule(scheduler.CronExpression)
                .WithDescription(scheduler.CronExpression)
                .Build();
        }
    }
}
