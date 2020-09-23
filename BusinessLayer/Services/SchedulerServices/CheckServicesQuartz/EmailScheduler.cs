using System;

namespace Library.Services.CheckServicesQuartz
{
    public class EmailScheduler
    {
        public Type JobType { get; }

        public string CronExpression { get; }

        public EmailScheduler(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }
    }
}
