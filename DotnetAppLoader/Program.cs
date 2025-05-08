using DotnetAppLoader;
using DotnetAppLoader.Grpc;


if (args.Length == 0)
{
    throw new ArgumentException("No command line arguments provided. Please provide the gRPC endpoint.");
}

Logger.LogInfo($"Raw Command line args: {string.Join(" ", args)}");


var grpcEndpoint = "";
if (args.Length > 0)
{
    grpcEndpoint = args[0];
}

try
{
    using (var appLoader = new AppLoader())
    {
        await new GrpcClient(grpcEndpoint, appLoader).InitAsync();
    }
}
catch (Exception ex)
{
    Console.WriteLine("Error calling RunApplication from Main." + ex.ToString());
}
finally
{
    Console.WriteLine("Exiting Main");
}

Console.ReadKey();
return 1;
