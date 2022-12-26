using CertificateUpdater.Config;
using CertificateUpdater.Logging;
using System.IO;
using WinSCP;

namespace CertificateUpdater.Controller
{
	public class SSHSaveController
	{
		private ILogger _log;

		public SSHSaveController(ILogger log)
		{
			_log = log;
		}

		public void Save(SSHSaveConfig config, string newCertificate)
		{
			SessionOptions sessionOptions = new SessionOptions
			{
				Protocol = Protocol.Sftp,
				HostName = config.Host,
				UserName = config.Username,
				Password = config.Password,
				SshHostKeyPolicy = SshHostKeyPolicy.GiveUpSecurityAndAcceptAny
			};

			using (Session session = new Session())
			{
				// Connect
				session.Open(sessionOptions);

				// Upload files
				TransferOptions transferOptions = new TransferOptions();
				transferOptions.TransferMode = TransferMode.Automatic;
				using (var stream = new MemoryStream())
				{
					var writer = new StreamWriter(stream);
					writer.Write(newCertificate);
					writer.Flush();
					stream.Position = 0;
					_log.LogInfo($"Uploading certificate to '{config.CertificatePath}' on '{config.Host}'");
					session.PutFile(stream, config.CertificatePath, transferOptions);
				}

			}
		}
	}
}