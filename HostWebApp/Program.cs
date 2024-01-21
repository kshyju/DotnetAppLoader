

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

            // Configure the HTTP request pipeline.
            app.MapGrpcService<Services.MyMessagingService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}