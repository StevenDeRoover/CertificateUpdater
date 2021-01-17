using CertificateUpdater.Config;
using CertrificateUpdater.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Config
{
    public class PFXSaveConfig : IConfig, ISaveCertificateConfig
    {
        public string Path { get; set; }
        public string Password { get; set; }
        public string Alias { get; set; }
        public PFXSaveController Controller { get; set; }
        public void Save(string newCertificate) => Controller.Save(this, newCertificate);
    }
}
