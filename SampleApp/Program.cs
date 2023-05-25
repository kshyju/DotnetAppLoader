namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (var i = 1; i <= 2; i++)
            {
                Console.WriteLine(" [SampleApp] Hello world - " + i);
                Thread.Sleep(200);
            }

            var nativeHost = new NativeWorkerClientFactory().CreateClient();
            Console.WriteLine($"NativeHost Application Ptr: {nativeHost.pNativeApplication}");

            for (var i = 1; i <= 5; i++)
            {

                Console.WriteLine(" [SampleApp] - " + i);
                Thread.Sleep(200);
            }
        }
    }
}