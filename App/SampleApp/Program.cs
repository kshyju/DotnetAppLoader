
namespace SampleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello from SampleApp main method");
            PrintEnvVariables();

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                PrintEnvVariables();
            }

            Console.WriteLine("Exiting SampleApp main method");
        }

        private static void PrintEnvVariables()
        {
            var currentEnvironemntVariableCount = Environment.GetEnvironmentVariables().Count;
            var aotFooValue = Environment.GetEnvironmentVariable("AOTFoo");
            Console.WriteLine($" [SampleAppManagedCode][{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Total env variable count:{currentEnvironemntVariableCount}. AOTFoo env variable value: {aotFooValue}");
        }
    }
}