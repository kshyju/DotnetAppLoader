using FunctionRpcGrpcService;
using HostWebApp;

namespace MovieService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();
            builder.Services.AddHostedService<ProcessStarterHostedService>();
            var app = builder.Build();

            app.MapGrpcService<MyMessagingService>();

            app.Run();
        }
    }
}