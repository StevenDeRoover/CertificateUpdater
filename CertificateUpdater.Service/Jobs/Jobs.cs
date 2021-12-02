using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Service.Jobs
{
    public static class Jobs
    {
        public static void EnqueueJobs()
        {
            RecurringJob.AddOrUpdate<Application>(nameof(Application), app => app.Run(false), Cron.Daily, TimeZoneInfo.Local);
        }
    }
}
