using Autofac;
using Autofac.Extras.AttributeMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Logging;

namespace CertificateUpdater.DI
{
    public static class Container
    {
        public static IContainer CompositionRoot(Action<ContainerBuilder> config = null)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AttributedMetadataModule>();
            builder.RegisterModule<InjectionModule>();
            builder.RegisterType<Application>().PropertiesAutowired();
            config?.Invoke(builder);
            return builder.Build();
        }
    }
}
