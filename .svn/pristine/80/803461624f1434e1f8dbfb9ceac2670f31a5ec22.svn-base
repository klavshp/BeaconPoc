using System;
using System.Configuration;
using System.IO;

namespace BeaconPocService
{
    internal static class Logger
    {
        internal static void LogError(string logMessage, bool headers = false)
        {
            using (var logFile = File.AppendText(ConfigurationManager.AppSettings["logDir"] + "/" + "BeaconPocLog.txt"))
            {
                if (headers) logFile.WriteLine("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                if (headers) logFile.WriteLine("---------------------------------------------------------------------------");
                if (logMessage.Length > 0) logFile.WriteLine("{0}", logMessage);
                logFile.Close();
            }
        }

        internal static void LogInfo(string logMessage, bool headers = false)
        {
            using (var logFile = File.AppendText(ConfigurationManager.AppSettings["logDir"] + "/" + "BeaconPocLog.txt"))
            {
                if (headers) logFile.WriteLine("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                if (headers) logFile.WriteLine("---------------------------------------------------------------------------");
                if (logMessage.Length > 0) logFile.WriteLine("{0}", logMessage);
                logFile.Close();
            }
        }
    }
}