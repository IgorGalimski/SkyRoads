using System;
using System.IO;
using UnityEngine;

namespace SpaceShooter.Managers
{
    public static class LogManager
    {
        private const string LOG_FORMAT = "{0} [{1}] {2}";

        private const string LOG_FILENAME = "log.txt";

        private static string _logPath;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            _logPath = Path.Combine(Application.persistentDataPath, LOG_FILENAME);

            Application.logMessageReceived += ApplicationOnLogMessageReceived;
        }

        private static void ApplicationOnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            File.AppendAllText(_logPath,
                string.Format(LOG_FORMAT, DateTime.Now, type, condition) + Environment.NewLine);
        }
    }
}