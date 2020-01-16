using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Config;
using CertificateUpdater.Controller;
using CertificateUpdater.Logging;

namespace CertificateUpdater.Tests
{

    [TestClass]
    public class SSHSaveTest
    {
        Mock<ILogger> _logger;

        [TestInitialize]
        public void Setup()
        {
            _logger = new Mock<ILogger>(MockBehavior.Loose);
        }
        

        [TestMethod]
        public void NotifyDnsChanges_Integration()
        {
            var controller = new SSHSaveController(_logger.Object);
            var config = new SSHSaveConfig() { Host = "stovem.com", Username = "root", Password = "k@st33ltj3", CertificatePath = "/www/private/certificate.test.pem" };

            controller.Save(config, "TESTCERTIFICATE");
        }
    }
}
