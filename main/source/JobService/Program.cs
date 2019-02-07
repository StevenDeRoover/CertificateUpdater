using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace JobService
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> theArgs = new List<string>();
            var rc = HostFactory.Run(x =>
            {
                x.Service<JobService>(s =>
                {
                    s.ConstructUsing(name => new JobService(theArgs.ToArray()));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalService();

                x.SetDescription("Hangfire JobService");
                x.SetDisplayName("JobService");
                x.SetServiceName("JobService");
                x.AddCommandLineDefinition("port", v => { theArgs.Add("port"); theArgs.Add(v); });
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
            Console.Read();
        }
    }
}
