using Owin;
using System.Web.Http;
using System;
using Quartzmin;
using Quartz.Impl;
using JobService.Scheduler;

class Startup
{
    // This code configures Web API. The Startup class is specified as a type
    // parameter in the WebApp.Start method.
    public void Configuration(IAppBuilder app)
    {
        HttpConfiguration config = new HttpConfiguration();



        //  Enable attribute based routing
        config.MapHttpAttributeRoutes();

        //app.UseWebApi(config);

        app.UseQuartzmin(new QuartzminOptions()
        {
            Scheduler = JobScheduler.Create().Result,
        });
        
    }
}