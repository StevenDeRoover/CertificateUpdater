using CertificateUpdater.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.SCP
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var host = Input("Host", "192.168.1.1");
			var userName = Input("Username", "root");
			var password = Input("Password");
			var file = Input("Filename", "d:\\documents\\certificate.pem");
			var scpPath = Input("SCP path", "/www/private/certificate.pem");
			var command = Input("Command", "/etc/init.d/haproxy restart");
			{
				var config = new Config.SSHSaveConfig()
				{
					CertificatePath = scpPath,
					Host = host,
					Username = userName,
					Password = password
				};
				var controller = new Controller.SSHSaveController(new ConsoleLogger());
				controller.Save(config , System.IO.File.ReadAllText(file));
			}

			if(!string.IsNullOrWhiteSpace(command)) 
			{
				var config = new Config.SSHCommandNotifyConfig()
				{
					Host = host,
					Username = userName,
					Password = password,
				};
				var controller = new Controller.SSHCommandNotifyController(new ConsoleLogger());
				controller.Execute(config, command);
			}
		}

		static string Input(string message, string @default = null)
		{
			Console.Write($"{message}{(string.IsNullOrWhiteSpace(@default) ? "" : $"({@default})")} :");
			var result = Console.ReadLine();
			return string.IsNullOrEmpty(result) ? @default : result;
		}
	}
}
