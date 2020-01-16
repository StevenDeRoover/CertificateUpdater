using Autofac;
using Autofac.Integration.WebApi;
using CertificateUpdater.DI.Extensions;
using CertificateUpdater.Logging;
using CertificateUpdater.Service.Logging;
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
        // This method is required by Katana:
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();

            // Use the extension method provided by the WebApi.Owin library:
            app.UseWebApi(webApiConfiguration);
        }


        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();

            var container = DI.Container.CompositionRoot(builder =>
            {
                builder.Register((c, p) => new CompositeLogger(new ILogger[] { new NLogger(), new TraceLogger() })).As<ILogger>();
                builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            }).UseConfig("config.json");

            var dependencyResolver = new AutofacWebApiDependencyResolver(container);
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
