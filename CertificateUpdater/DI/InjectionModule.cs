using Autofac;
using Autofac.Features.AttributeFilters;
using Certes;
using CertificateUpdater.Acme;
using CertificateUpdater.Config;
using CertificateUpdater.Controller;
using CertificateUpdater.Http;

namespace CertificateUpdater.DI
{
    class InjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.RegisterType<Configuration>().AsSelf().SingleInstance().PropertiesAutowired();
            
            builder.RegisterType<AcmeConfig>().Keyed<IConfig>("Acme").PropertiesAutowired();
            builder.RegisterType<UrlConfig>().Keyed<IConfig>("Url").PropertiesAutowired();
            builder.RegisterType<MailNotifyConfig>().Keyed<IConfig>("MailNotify").PropertiesAutowired();
            builder.RegisterType<PushBulletNotifyConfig>().Keyed<IConfig>("PushBulletNotify").PropertiesAutowired();
            builder.RegisterType<SSHSaveConfig>().Keyed<IConfig>("SSHSave").PropertiesAutowired();
            builder.RegisterType<PFXSaveConfig>().Keyed<IConfig>("PFXSave").PropertiesAutowired();
            builder.RegisterType<SSHCommandNotifyConfig>().Keyed<IConfig>("SSHCommandNotify").PropertiesAutowired();

            //acme
            builder.RegisterType<AcmeFactory>().AsSelf();
            builder.RegisterType<DnsValidator>().Keyed<IValidator>(AcmeConfig.ValidationType.DNS);
            builder.RegisterType<HttpValidator>().Keyed<IValidator>(AcmeConfig.ValidationType.HTTP);

            builder.Register((c, p) =>
            {
                return c.Resolve<AcmeFactory>().CreateContext(p.TypedAs<AcmeConfig>()).Result;
            }).As<AcmeContext>();

            //controllers
            builder.RegisterAssemblyTypes(ThisAssembly).Where(c => c.Name.EndsWith("Controller")).AsSelf().PropertiesAutowired().WithAttributeFiltering();
            builder.RegisterAssemblyTypes(ThisAssembly).Where(c => c.Name.EndsWith("Model")).AsSelf();

            //HttpClientFactory
            builder.RegisterType<HttpClientFactory>().AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}

