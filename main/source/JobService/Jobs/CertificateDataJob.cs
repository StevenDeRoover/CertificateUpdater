using CertificateUpdater;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.Jobs
{
    [PersistJobDataAfterExecution]
    [Description("This task does nothing, it just persists certificate data")]
    public class CertificateDataJob : IJob
    {
        public static JobKey JobKey = new JobKey("CertificateData");

        public CertificateDataJob()
        { }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                //update jobdatamap
                if (context.MergedJobDataMap.Any())
                {
                    foreach (var kv in context.MergedJobDataMap)
                    {
                        if (context.JobDetail.JobDataMap.ContainsKey(kv.Key))
                        {
                            context.JobDetail.JobDataMap[kv.Key] = kv.Value;
                        }
                        else
                        {
                            context.JobDetail.JobDataMap.Add(kv.Key, kv.Value);
                        }
                    } 
                }
            });
        }
    }
}
