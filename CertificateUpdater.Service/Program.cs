using Autofac;
using CertificateUpdater.DI;
using CertificateUpdater.DI.Extensions;
using CertificateUpdater.Logging;
using System;
using System.Configuration;
using Topshelf;

namespace CertificateUpdater.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            //var container = Container.CompositionRoot(builder =>
            //{
            //    builder.RegisterType<ConsoleLogger>().As<ILogger>();
            //}).UseConfig("config.json");

            var rc = HostFactory.Run(x =>
            {
                x.Service<Api.ApiService>(s =>
                {
                    s.ConstructUsing(name => new Api.ApiService(int.Parse(ConfigurationManager.AppSettings["Owin::Port"] ?? "8099")));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.SetDescription("Certificate updater for Let's Encrypt");
                x.SetDisplayName("CertificateUpdater");
                x.SetServiceName("CertificateUpdater");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
