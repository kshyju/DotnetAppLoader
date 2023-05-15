using DotnetAppLoader;

class Program
{
    static int Main(string[] args)
    {
        var workerAssemblyPath = @"D:\src\misc\DotnetAppLoader\SampleApp\bin\Debug\net6.0\SampleApp.dll";

        return AppLoader.RunApplication(workerAssemblyPath);
    }
}
