using CertificateUpdater.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Service.Logging
{
    public class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger(ILogger[] loggers)
        {
            _loggers = new List<ILogger>(loggers);
        }

        public void LogError(Exception ex)
        {
            _loggers.ForEach(l => l.LogError(ex));
        }

        public void LogInfo(string message)
        {
            _loggers.ForEach(l => l.LogInfo(message));
        }

        public void LogWarning(string message)
        {
            _loggers.ForEach(l => l.LogWarning(message));
        }
    }
}
