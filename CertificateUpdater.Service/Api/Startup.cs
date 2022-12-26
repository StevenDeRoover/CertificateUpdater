using Autofac;
using Autofac.Integration.WebApi;
using CertificateUpdater.DI.Extensions;
using CertificateUpdater.Logging;
using CertificateUpdater.Service.Logging;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CertificateUpdater.Service.Api
{
	public class Startup
	{
		private IContainer _container;


		private void SetupDI()
		{
			_container = DI.Container.CompositionRoot(builder =>
			{
				builder.Register((c, p) => new CompositeLogger(new ILogger[] { new NLogger(), new TraceLogger() })).As<ILogger>();
				builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
			}).UseConfig("config.json");
		}

		// This method is required by Katana:
		public void Configuration(IAppBuilder app)
		{
			SetupDI();
			var webApiConfiguration = ConfigureWebApi();

			// Use the extension method provided by the WebApi.Owin library:
			app.UseWebApi(webApiConfiguration);

			//Setup hangfire
			GlobalConfiguration.Configuration.UseMemoryStorage();
			GlobalConfiguration.Configuration.UseAutofacActivator(_container);
			GlobalConfiguration.Configuration.UseNLogLogProvider();
			var dboptions = new DashboardOptions()
			{
				Authorization = new IDashboardAuthorizationFilter[] { new HangFireAuthorizationFilter { User = "admin", Pass = "@dministr@tor" } }
			};
			app.UseHangfireServer();
			app.UseHangfireDashboard("", dboptions);
			Jobs.Jobs.EnqueueJobs();
		}


		private HttpConfiguration ConfigureWebApi()
		{
			var config = new HttpConfiguration();
			var dependencyResolver = new AutofacWebApiDependencyResolver(_container);
			config.DependencyResolver = dependencyResolver;

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				"DefaultApi",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional });
			return config;
		}
	}
}
