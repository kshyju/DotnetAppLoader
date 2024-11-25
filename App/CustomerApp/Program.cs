using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace SampleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("[CustomerApp] Hello from main method.");

            var host = new HostBuilder()
                    .ConfigureFunctionsWorkerDefaults()
                    .Build();

            for (var i = 0; i < 5; i++)
            {
                await Task.Delay(1000);
                Console.WriteLine($"[CustomerApp] Hello {i}");
            }
        }
    }
}