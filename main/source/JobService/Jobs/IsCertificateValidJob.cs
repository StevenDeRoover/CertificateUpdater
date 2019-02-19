using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Jobs
{
    public class IsCertificateValidJob : IJob
    {
        public static JobKey JobKey = new JobKey("IsCertificateValid");

        public IsCertificateValidJob()
        { }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () => {
                var jobDetail = await context.Scheduler.GetJobDetail(CertificateDataJob.JobKey);
                var jobDataMap = jobDetail.JobDataMap;

                if (!jobDataMap.ContainsKey("IsCertificateValid"))
                {
                    jobDataMap.Add("IsCertificateValid", false);
                }

                var certificateChecker = new CertificateUpdater.CertificateChecker(jobDataMap["AccountKey"].ToString(), jobDataMap["Certificate"].ToString());
                var shouldRenewCertificate = await certificateChecker.CheckShouldRenewCertificate();
                jobDataMap["IsCertificateValid"] = !shouldRenewCertificate;

                await context.Scheduler.TriggerJob(CertificateDataJob.JobKey, jobDataMap);
            });
        }
    }
}
