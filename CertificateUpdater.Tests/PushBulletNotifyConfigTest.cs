using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using CertificateUpdater.Config;
using CertificateUpdater.Controller;
using CertificateUpdater.Http;

namespace CertificateUpdater.Tests
{
    [TestClass]
    public class PushBulletNotifyConfigTest
    {
        [TestMethod]
        public void NotifyDnsChanges()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("[{'id':1,'value':'1'}]"),
               })
               .Verifiable();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var httpClient = new HttpClient(handlerMock.Object);

            httpClientFactoryMock.Setup(m => m.CreateClient()).Returns(httpClient);

            PushBulletNotifyController controller = new PushBulletNotifyController((IHttpClientFactory)httpClientFactoryMock.Object);

            Dictionary<string, string> dnsValidations = new Dictionary<string, string>();
            dnsValidations.Add("www.google.be", "azerty");
            dnsValidations.Add("www.google.com", "qwerty");
            var config = new PushBulletNotifyConfig() { AccountKey = "ABC" };
            controller.NotifyDnsChanges(config, dnsValidations);

            httpClientFactoryMock.Verify(m => m.CreateClient(), Times.Once);
            handlerMock.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public void NotifyDnsChanges_Integration()
        {
            PushBulletNotifyController controller = new PushBulletNotifyController(new HttpClientFactory());

            Dictionary<string, string> dnsValidations = new Dictionary<string, string>();
            dnsValidations.Add("www.google.be", "azerty");
            dnsValidations.Add("www.google.com", "qwerty");
            var config = new PushBulletNotifyConfig() { AccountKey = "o.R9eM7I2TEUBRS35b467vp8d46Q07ag87" };
            controller.NotifyDnsChanges(config, dnsValidations);
        }

    }
}
