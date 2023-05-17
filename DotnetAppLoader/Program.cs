using DotnetAppLoader;

class Program
{
    static int Main(string[] args)
    {
        var workerAssemblyPath = @"C:\Dev\OSS\DotnetAppLoader\SampleApp\bin\Debug\net7.0\SampleApp.dll";

        if (args.Length > 0)
        {
            workerAssemblyPath = args[0];
        }

        return AppLoader.RunApplication(workerAssemblyPath);
    }
}
