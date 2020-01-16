using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Http;
using CertificateUpdater.Models;

namespace CertificateUpdater.Controller
{
    public class PushBulletNotifyController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationModel Model { get; set; }

        public PushBulletNotifyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void NotifyDnsChanges(Config.PushBulletNotifyConfig config, Dictionary<string, string> dnsValidations)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            foreach (var dnsValidation in dnsValidations)
            {
                var body = new {
                    body = $"{dnsValidation.Key}={dnsValidation.Value}",
                    title = "Change your DNS",
                    type = "note"
                };
                var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                var content = new System.Net.Http.StringContent(stringContent, Encoding.UTF8, "application/json");

                content.Headers.Add("Access-Token", config.AccountKey);

                var post = client.PostAsync("https://api.pushbullet.com/v2/pushes", content).Result;
                var result = post.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
