namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (var i = 1; i <= 2; i++)
            {
                Console.WriteLine("Hello world - " + i);
                Thread.Sleep(1000);
            }

            var nativeHost = new NativeWorkerClientFactory().CreateClient();
            Console.WriteLine($"NativeHost Application Ptr: {nativeHost.pNativeApplication}");

            for (var i = 1; i <= 2; i++)
            {
                Console.WriteLine("Yo0 world - " + i);
                Thread.Sleep(1000);
            }
        }
    }
}