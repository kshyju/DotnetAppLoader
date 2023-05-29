namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.LogInfo($"AppDomain.CurrentDomain.BaseDirectory: {AppDomain.CurrentDomain.BaseDirectory}");
            Logger.LogInfo($"Assembly Location: {typeof(Program).Assembly.Location}");

            // If you want to inspect something from the environment before our code crashes, set this env variable
            // so that it runs this loop before calling CreateClient()
            var dontCrashEnvVarValue = Environment.GetEnvironmentVariable("DONTCRASH");

            if (dontCrashEnvVarValue != null)
            {
                for (var i = 1; i <= 5000; i++)
                {
                    Logger.LogInfo(" - " + i);
                    Thread.Sleep(1000);
                }
            }

            var nativeHostData = NativeMethods.GetNativeHostData();

            Logger.LogInfo($"NativeHost Application Ptr: {nativeHostData.pNativeApplication}");
        }
    }
}