using System.Globalization;

namespace DotnetAppLoader
{
    internal class Logger
    {
        const string prefix = " ";
        public static void Log(string message)
        {
            string ts = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.WriteLine($"{prefix}[{ts}] [DotnetAppLoader] {message}");
        }
    }
}
