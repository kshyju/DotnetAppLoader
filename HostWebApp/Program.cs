using FunctionRpcGrpcService;
using HostWebApp;

namespace MovieService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5000); // HTTP for web app
                options.ListenAnyIP(6000, listenOptions =>
                {
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                }); // gRPC on port 6000
            });

            builder.Services.AddGrpc();
            builder.Services.AddHostedService<ProcessStarterHostedService>();
            var app = builder.Build();

            app.MapGrpcService<MyMessagingService>();

            app.Run();
        }
    }
}