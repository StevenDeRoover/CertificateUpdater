using Owin;
using Hangfire;
using Hangfire.LiteDB;
using Hangfire.Console;
using System.Web.Http;
using System;
using JobService.Job;

class Startup
{
    // This code configures Web API. The Startup class is specified as a type
    // parameter in the WebApp.Start method.
    public void Configuration(IAppBuilder app)
    {
        GlobalConfiguration.Configuration.UseConsole().UseLiteDbStorage();

        HttpConfiguration config = new HttpConfiguration();

        //  Enable attribute based routing
        config.MapHttpAttributeRoutes();

        app.UseWebApi(config);

        // Configure Web API for self-host. 
        app.UseHangfireServer();
        app.UseHangfireDashboard();
        SetupRecurringJobs();
    }

    private void SetupRecurringJobs()
    {
        
        RecurringJob.AddOrUpdate<CheckCertificateJob>(CheckCertificateJob.Id, x => x.Check(null), Cron.Hourly);
    }
}