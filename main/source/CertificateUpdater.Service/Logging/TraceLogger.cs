using CertificateUpdater.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Service.Logging
{
    internal class TraceLogger : ILogger
    {
        public void LogError(Exception ex)
        {
            Trace.TraceError(ex.Message + "\r\n" + ex.StackTrace);
        }

        public void LogInfo(string message)
        {
            Trace.TraceInformation(message);
        }

        public void LogWarning(string message)
        {
            Trace.TraceWarning(message);
        }
    }
}
