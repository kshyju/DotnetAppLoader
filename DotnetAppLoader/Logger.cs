using System.Globalization;

namespace DotnetAppLoader
{
    internal static class Logger
    {
        public static void LogInfo(string message)
        {
            var ts = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.WriteLine($"[DotnetAppLoader] {ts} {message}");
        }

        internal static void LogDebug(string v) => LogInfo(v);
    }
}
