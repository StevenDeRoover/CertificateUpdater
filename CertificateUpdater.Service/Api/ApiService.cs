using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Service.Api
{
    public class ApiService
    {
        private int port;
        private IDisposable _host;

        public ApiService(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            string baseUri = $"http://+:{this.port}/";

            Console.WriteLine("Starting web Server...");
            _host = WebApp.Start<Startup>(baseUri);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);
#if DEBUG
            Console.ReadLine();
#endif
        }

        public void Stop()
        {
            _host?.Dispose();
        }
    }
}
