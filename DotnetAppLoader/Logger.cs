using System.Globalization;

namespace DotnetAppLoader
{
    internal static class Logger
    {
        internal static void LogInfo(string message)
        {
            var ts = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var logMessage = $"[DotnetAppLoaderNativeCode][{ts}] {message}";

            Console.WriteLine(logMessage);
        }
    }
}
