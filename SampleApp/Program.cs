namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appDomainBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var assemblyLocation = typeof(SampleApp.Program).Assembly.Location;
            Console.WriteLine($" [SampleApp] appDomainBaseDirectory: {appDomainBaseDirectory}");
            Console.WriteLine($" [SampleApp] assemblyLocation: {assemblyLocation}");

            //for (var i = 1; i <= 5000; i++)
            //{
            //    Console.WriteLine(" [SampleApp] - " + i);
            //    Thread.Sleep(1000);
            //}

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