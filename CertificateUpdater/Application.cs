using CertificateUpdater.Controller;
using System;

namespace CertificateUpdater
{
    public class Application : IDisposable
    {
        public CertificateController CertificateController { get; set; }

        public Application() 
        {
        }

        public void Run()
        {
            CertificateController.ProcessCertificates();
        }

        public void Dispose()
        {
        }
    }
}
