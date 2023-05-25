using System.Globalization;

namespace DotnetAppLoader
{
    internal class Logger
    {
        const string prefix = " ";
        public static void LogInfo(string message)
        {
            string ts = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.WriteLine($"{prefix}[{ts}] [DotnetAppLoader] {message}");
        }

        internal static void LogDebug(string v)
        {
            LogInfo(v);
        }
    }
}
