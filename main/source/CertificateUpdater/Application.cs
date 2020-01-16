﻿using CertificateUpdater.Controller;
using CertificateUpdater.Logging;
using System;

namespace CertificateUpdater
{
    public class Application : IDisposable
    {
        private readonly ILogger _log;

        public CertificateController CertificateController { get; set; }

        public Application(ILogger log) 
        {
            _log = log;
        }

        public void Run()
        {
            _log.LogInfo("Application run");
            CertificateController.ProcessCertificates();
        }

        public void Dispose()
        {
        }
    }
}
