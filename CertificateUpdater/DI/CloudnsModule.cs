using Autofac;
using CloudnsAPI.Client;
using CloudnsAPI.Client.Abstractions;
using CloudnsAPI.Client.Authentication;

namespace CertrificateUpdater.DI
{
	internal class CloudnsModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<Client>();
			builder.RegisterType<DelegateAuthenticator>().As<IAuthenticator>();
		}
	}
}
