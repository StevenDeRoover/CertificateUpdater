using Autofac;
using CloudnsAPI.Client;
using CloudnsAPI.Client.Abstractions;
using CloudnsAPI.Client.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
