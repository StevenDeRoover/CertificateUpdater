using JobService.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Scheduler
{
    public class JobScheduler
    {
        public static async Task<IScheduler> Create()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteClient";
            //Quartz.Plugins.RecentHistory.ExecutionHistoryPlugin
            // set thread pool info            
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.plugin.recentHistory.type"] = "Quartz.Plugins.RecentHistory.ExecutionHistoryPlugin, Quartz.Plugins.RecentHistory";
            properties["quartz.plugin.recentHistory.storeType"] = "Quartz.Plugins.RecentHistory.Impl.InProcExecutionHistoryStore, Quartz.Plugins.RecentHistory";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            properties["quartz.serializer.type"] = "json";
            properties["jobStore.misfireThreshold"] = "60000";
            properties["quartz.jobStore.dataSource"] = "default";
            properties["quartz.jobStore.tablePrefix"] = "qrtz_";
            properties[".jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SQLiteDelegate, Quartz";
            properties["quartz.dataSource.default.provider"] = "SQLite";
            properties["quartz.dataSource.default.connectionString"] = "Data Source=c:\\temp\\quartznet.db;Version=3;";

            /*
             <add key="quartz.jobStore.type" value="Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" />
    <add key="quartz.jobStore.misfireThreshold" value="60000" />
    <add key="quartz.jobStore.dataSource" value="default" />
    <add key="quartz.jobStore.tablePrefix" value="qrtz_" />
    <add key="quartz.jobStore.driverDelegateType" value="Quartz.Impl.AdoJobStore.SQLiteDelegate, Quartz" />
    <add key="quartz.dataSource.default.provider" value="SQLite" />
    <add key="quartz.dataSource.default.connectionString" value="Data Source=E:\WebApplication1\App_Data\quartznet.db;Version=3;" />
             */

            var factory = new StdSchedulerFactory(properties);
            var scheduler = await factory.GetScheduler();
            
            if (!(await scheduler.CheckExists(CertificateDataJob.JobKey)))
            {
                var certificateDataJob = JobBuilder.Create<CertificateDataJob>().WithIdentity(CertificateDataJob.JobKey).StoreDurably().WithDescription("Do not remove!").Build();
                await scheduler.AddJob(certificateDataJob, false);
            }
            
            await scheduler.Start();
            return scheduler;
        }
    }
}
