using DotnetAppLoader;
using FunctionsNetHost.Grpc;

class Program
{

    static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Pass the .NET (customer )assembly path as argument. Ex: ./FunctionsNetHost C:/Temp/SampleApp.dll");
            return 1;
        }

        Logger.LogInfo($"Raw Command line args: {string.Join(" ", args)}");

        var customerAssemblyPath = args[0];

        var grpcEndpoint = "";
        if (args.Length>1)
        {
            grpcEndpoint = args[1];
        }

        grpcEndpoint = "http://localhost:5138";

        using (var appLoader = new AppLoader())
        {
            try
            {
                await new GrpcClient(appLoader, customerAssemblyPath, grpcEndpoint).InitAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling RunApplication from Main." + ex.ToString());
            }
        }
        Console.ReadKey();
        return 1;
    }
}
