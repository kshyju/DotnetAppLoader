using System.Collections;

namespace ConsoleApp1
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Log("Hello from SampleApp main method");

            for (int i = 30; i > 0; i--)
            {
                await Task.Delay(2000);
                PrintAllEnvironmentVariablesWithPrefix(i, "DOTNET_");
            }

            Log("Exiting SampleApp main method");
        }

        private static void PrintAllEnvironmentVariablesWithPrefix(int counter, string prefix)
        {
            Log($"{counter} Printing all environment variables with prefix: {prefix}");

            var envVariables = Environment.GetEnvironmentVariables();
            Log($"  Environment variables count: {envVariables.Count}");
            foreach (DictionaryEntry entry in envVariables)
            {
                var key = entry.Key?.ToString();
                var value = entry.Value?.ToString();

                if (key != null && key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    Log($"  Key: {key}, Value: {value}");
                }
            }
        }

        private static void Log(string message) => Console.WriteLine($"[ConsoleApp1][{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {message}");
    }
}
