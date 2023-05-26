namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appDomainBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var assemblyLocation = typeof(Program).Assembly.Location;

            Console.WriteLine($" [SampleApp] AppDomain.CurrentDomain.BaseDirectory: {appDomainBaseDirectory}");
            Console.WriteLine($" [SampleApp] Assembly Location: {assemblyLocation}");

            // If you want to inspect something from the environment before our code crashes, set this env variable
            // so that it runs this loop before calling CreateClient()
            var dontCrashEnvVarValue = Environment.GetEnvironmentVariable("DONTCRASH");
            Console.WriteLine($" [SampleApp] DONTCRASH EnvironmentVariable: {dontCrashEnvVarValue}");

            if (dontCrashEnvVarValue != null)
            {
                for (var i = 1; i <= 5000; i++)
                {
                    Console.WriteLine(" [SampleApp] - " + i);
                    Thread.Sleep(1000);
                }
            }

            var nativeHost = new NativeWorkerClientFactory().CreateClient();
            Console.WriteLine($" [SampleApp] NativeHost Application Ptr: {nativeHost.pNativeApplication}");
        }
    }
}