using System.Globalization;

namespace DotnetAppLoader
{
    internal static class Logger
    {
        private static string? _logFilePath;

        static Logger()
        {
            CreateLogFile();
        }

        private static void CreateLogFile()
        {
            var logFilePath = """C:\\devtools\apploader.txt""";

            var pid = Environment.ProcessId;
            var fileExist = File.Exists(logFilePath);
            if (!fileExist)
            {
                try
                {
                    File.AppendAllText(logFilePath, $"{Environment.NewLine}Log file created at {DateTime.UtcNow}(UTC){Environment.NewLine}PID:{pid}{Environment.NewLine}");
                    fileExist = true;
                    _logFilePath = logFilePath;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating log file at {logFilePath}: {ex.Message}");
                }
                return;
            }
            else
            {
                _logFilePath = logFilePath;
                File.AppendAllText(_logFilePath, $"{Environment.NewLine}{Environment.NewLine}PID:{pid}{Environment.NewLine}");
            }
        }

        internal static void LogInfo(string message)
        {
            var ts = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var logMessage = $"[DotnetAppLoader][{ts}] [FunctionsNetHost] {message}";

            if (!string.IsNullOrEmpty(_logFilePath))
            {
                try
                {
                    File.AppendAllText(_logFilePath, $"{logMessage}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }

            Console.WriteLine(logMessage);
        }
        internal static void LogDebug(string v) => LogInfo(v);
    }
}
