using AppLibrary;

namespace SampleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Logger.LogInfo($"Inside SampleApp Main");
            var os = Environment.OSVersion;
            Logger.LogInfo($"OS: {os}");

            new NativeClient().Start();

            // Wait indefinitely
            await Task.Delay(Timeout.Infinite);
        }
    }
}