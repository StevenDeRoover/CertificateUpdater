using Hangfire;
using Hangfire.Server;
using Hangfire.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JobService.Extensions;

namespace JobService.Job
{
    public class CheckCertificateJob
    {
        public static string Id = nameof(CheckCertificateJob);
        public void Check(PerformContext context)
        {
            var viewBag = context.GetViewBag();

            context.WriteLine(ConsoleTextColor.Blue, "Start check certificate job");   
        }
    }
}
