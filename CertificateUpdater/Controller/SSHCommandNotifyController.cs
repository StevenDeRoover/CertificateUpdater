using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Config;
using CertificateUpdater.Logging;

namespace CertificateUpdater.Controller
{
    public class SSHCommandNotifyController
    {
        private readonly ILogger _log;

        public SSHCommandNotifyController(ILogger log)
        {
            _log = log;
        }

        public void Execute(SSHCommandNotifyConfig config, string command)
        {
            var connectionInfo = new ConnectionInfo(config.Host, config.Username, new PasswordAuthenticationMethod(config.Username, (string)config.Password));
            using (var client = new SshClient(connectionInfo))
            {
                client.Connect();
                _log.LogInfo($"Executing SSH Command '{command}'");
                var result = client.RunCommand(command);
                _log.LogInfo(result.Result);
            }
        }
    }
}
