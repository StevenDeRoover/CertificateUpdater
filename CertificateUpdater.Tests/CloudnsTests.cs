using CloudnsAPI.Client;
using CloudnsAPI.Client.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CertificateUpdater.Tests
{
	[TestClass]
	public class CloudnsTests
	{
		private Client GetClient()
		{
			var authenticator = new DelegateAuthenticator(new DelegateAuthenticationOptions());
			var client = new Client(authenticator, new LoggingHandler());
			client.Key = "8823";
			client.Password = "J@nDR_5445";
			return client;
		}

		[TestMethod]
		public void Cloudns_Login()
		{
			var login = GetClient().Login("8823", "J@nDR_5445").Result;
			Assert.AreEqual(login.Status, "Success");
		}

		private class LoggingHandler : DelegatingHandler
		{
			public LoggingHandler()
			{
				InnerHandler = new HttpClientHandler();
			}
			protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				var result = await base.SendAsync(request, cancellationToken);
				var content = await result.Content.ReadAsStringAsync();
				return result;
			}
		}

		[TestMethod]
		public void Cloudns_DNSList()
		{
			var client = GetClient();
			var zones = client.DNS.Request().GetAsync().Result;
			Assert.IsNotNull(zones);
		}

		[TestMethod]
		public void Cloudns_DNSRecordsList()
		{
			var client = GetClient();
			var zones = client.DNS.Zone["stovem.com"].Records.Request().GetAsync().Result;
			Assert.IsNotNull(zones);
		}

		[TestMethod]
		public void Cloudns_DNSRecord_Update()
		{
			var client = GetClient(); 
			var zones = client.DNS.Zone["stovem.com"].Records.Request().GetAsync().Result;
			
			var acme = zones.FirstOrDefault(v => v.Value.Host == "_acme-challenge");
			Assert.IsNotNull(acme.Key);
			Assert.IsNotNull(acme.Value);
			acme.Value.Record = "TEST";
			client.DNS.Zone["stovem.com"].Records[acme.Key].Request().PostAsync(acme.Value).Wait();
		}
	}
}
