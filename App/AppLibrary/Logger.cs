using System;
using System.Globalization;

namespace AppLibrary
{
    public static class Logger
    {
        public static void LogInfo(string message)
        {
            var ts = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

            Console.WriteLine($"  [App] {ts} {message}");
        }
    }
}
