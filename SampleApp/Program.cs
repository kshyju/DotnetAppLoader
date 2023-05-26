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

            var dontCrashEnvVarValue = Environment.GetEnvironmentVariable("DONTCRASH");
            Console.WriteLine($" [SampleApp] DONTCRASH EnvironmentVariable: {dontCrashEnvVarValue}");

            if ( dontCrashEnvVarValue != null )
            {
                // If "CreateClient" is crashing,
                // Run this loop so that we can inspect the container before it crashes.
                for (var i = 1; i <= 5000; i++)
                {
                    Console.WriteLine(" [SampleApp] - " + i);
                    Thread.Sleep(1000);
                }
            }

            var nativeHost = new NativeWorkerClientFactory().CreateClient();
            Console.WriteLine($" [SampleApp] NativeHost Application Ptr: {nativeHost.pNativeApplication}");

            for (var i = 1; i <= 5000; i++)
            {
                Console.WriteLine(" [SampleApp] - " + i);
                Thread.Sleep(1000);
            }
        }
    }
}