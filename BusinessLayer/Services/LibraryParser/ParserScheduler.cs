using System;


namespace BusinessLayer.Services.LibraryParser
{
    public class ParserScheduler
    {
        public Type JobType { get; }

        public string CronExpression { get; }

        public ParserScheduler(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }
    }
}
