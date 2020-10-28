using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(Exception ex)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            LogInfo(ex.Message);
            LogInfo(ex.StackTrace);
            Console.ForegroundColor = color;
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            LogInfo(message);
            Console.ForegroundColor = color;
        }
    }
}
