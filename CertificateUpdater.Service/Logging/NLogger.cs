using CertificateUpdater.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Service.Logging
{
    public class NLogger : CertificateUpdater.Logging.ILogger
    {
        private static Logger _log;

        public NLogger()
        {
            CreateLogger();
        }

        private static void CreateLogger()
        {
            if (_log == null) _log = NLog.LogManager.GetLogger("Logging");
        }

        public void LogError(Exception ex)
        {
            _log.Error(ex, ex.Message);
        }

        public void LogInfo(string message)
        {
            _log.Info(message);
        }

        public void LogWarning(string message)
        {
            _log.Warn(message);
        }
    }
}
