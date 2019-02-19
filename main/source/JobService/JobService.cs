using JobService.Logging;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobService
{
    public class JobService
    {
        private string[] _args;

        public JobService(string[] args)
        {
            _args = args;
        }

        public JobService(string port)
        {

        }

        public void Start()
        {
            Logger.Log(TraceEventType.Start, "JobService service started");
            string port = "5150";
            var index = Array.IndexOf(_args, "port");
            if (index >= 0)
            {
                if (_args.Length >= index)
                {
                    port = _args[index + 1];
                }
            }
            Logger.LogVerbose($"Using port {port}");
            WebApp.Start<Startup>($"http://+:{port}");
            Logger.LogVerbose($"Server listening at http://localhost:{port}");
        }

        public void Stop()
        {
            Logger.Log(TraceEventType.Stop, "JobService service stopped");
        }
    }
}
