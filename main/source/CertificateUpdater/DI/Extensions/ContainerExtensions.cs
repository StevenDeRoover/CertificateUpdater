using Autofac;
using Newtonsoft.Json;
using System;
using CertificateUpdater.Config;
using CertificateUpdater.Json;

namespace CertificateUpdater.DI.Extensions
{
    public static class ContainerExtensions
    {
        public static IContainer WithContainer(this IContainer container, Action<IContainer> action)
        {
            action(container);
            return container;
        }

        public static IContainer UseConfig(this IContainer container, string configFile = null)
        {
            container.WithContainer(c =>
            {
                var config = System.IO.File.ReadAllText(configFile ?? "config.json");
                Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(config, new JsonSerializerSettings
                {
                    ContractResolver = new AutofacContractResolver(c)
                });
            });
            
            return container;
        }
    }
}
