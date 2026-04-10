using System;
using System.IO;
using System.Diagnostics;

namespace Book_House.Logging
{
    public static class AppLogger
    {
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private static readonly string LogFile = Path.Combine(LogDirectory, "app.log");

        static AppLogger()
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);
            }
            catch
            {

            }
        }

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Warn(string message)
        {
            Write("WARN", message);
        }
        public static void Error(string message)
        {
            Write("ERROR", message);
        }

        private static void Write(string level, string message)
        {
            try
            {
                var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
                File.AppendAllText(LogFile, line + Environment.NewLine);
                Debug.WriteLine(line);
            }
            catch
            {

            }
        }
    }
}
