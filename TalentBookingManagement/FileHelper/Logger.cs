using System;
using System.IO;

namespace TalentBookingManagement
{
    public static class Logger
    {
        private static readonly string logFilePath = "app_log.txt";

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        public static void Info(string message)
        {
            Log(message, LogLevel.Info);
        }

        public static void Warning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public static void Error(string message)
        {
            Log(message, LogLevel.Error);
        }
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}
