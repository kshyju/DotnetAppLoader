using DotnetAppLoader;

class Program
{
    static void Main(string[] args)
    {
        var workerAssemblyPath = @"C:\Dev\OSS\DotnetAppLoader\SampleApp\bin\Debug\net7.0\SampleApp.dll";

        new AppLoader().Load(workerAssemblyPath, "7.0");

    }
}
