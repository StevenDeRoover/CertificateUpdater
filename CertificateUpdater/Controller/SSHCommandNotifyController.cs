using CertificateUpdater.Config;
using CertificateUpdater.Logging;
using System;
using WinSCP;

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
			SessionOptions sessionOptions = new SessionOptions
			{
				Protocol = Protocol.Scp,
				HostName = config.Host,
				UserName = config.Username,
				Password = config.Password,
                SshHostKeyPolicy = SshHostKeyPolicy.GiveUpSecurityAndAcceptAny
			};

			using (Session session = new Session())
			{
				// Connect
				session.Open(sessionOptions);

                _log.LogInfo($"Executing SSH Command '{command}'");
                var result = session.ExecuteCommand(command);
                _log.LogInfo(result.Output);
            }
        }
    }
}
