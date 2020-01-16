using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Config
{
    public interface ISaveCertificateConfig
    {
        void Save(string newCertificate);
    }
}
