using Library.Models;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using System;

namespace Library.Services.CheckServicesQuartz
{
    public class EmailScheduler 
    {
        public EmailScheduler(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }

        public Type JobType { get; }
        public string CronExpression { get; }

    }
}
