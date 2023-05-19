using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureGeneratedFunctionExecutor()
    .ConfigureGeneratedFunctionMetadataProvider()
    .Build();

host.Run();
