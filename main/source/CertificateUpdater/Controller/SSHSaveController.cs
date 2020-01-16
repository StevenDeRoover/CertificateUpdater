using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Config;
using CertificateUpdater.Logging;

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
            var connectionInfo = new ConnectionInfo(config.Host, config.Username, new PasswordAuthenticationMethod(config.Username, config.Password));

            using (var client = new ScpClient(connectionInfo)) 
            {
                client.Connect();
                using (var stream = new MemoryStream())
                {
                    var writer = new StreamWriter(stream);
                    writer.Write(newCertificate);
                    writer.Flush();
                    stream.Position = 0;
                    _log.LogInfo($"Uploading certificate to '{config.CertificatePath}' on '{config.Host}'");
                    client.Upload(stream, config.CertificatePath);
                }
                
            }

        }
    }
}
