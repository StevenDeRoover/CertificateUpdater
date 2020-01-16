using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Controller;

namespace CertificateUpdater.Config
{
    public class SSHSaveConfig : IConfig, ISaveCertificateConfig
    {
        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public SSHSaveController Controller { get; set; }
        public string CertificatePath { get; set; }

        public void Save(string newCertificate) => Controller.Save(this, newCertificate);
    }
}
