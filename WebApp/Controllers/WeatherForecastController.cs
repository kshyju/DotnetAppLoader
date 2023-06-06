using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await Task.Delay(10);

            Console.WriteLine($"Environment.OSVersion.Platform: {Environment.OSVersion.Platform}");
            Console.WriteLine($"Environment.ProcessId: {Environment.ProcessId}");

            var path = @"C:\Temp\app\dotnetapploader\FunctionsNetHost.exe";
            var dir = @"C:\Temp\app\dotnetapploader";
            var dllPath = @"..\sampleapp\SampleApp.dll";

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                path = @"/mnt/c/Temp/app/dotnetapploader/FunctionsNetHost";
                dir = @"/mnt/c/Temp/app/dotnetapploader";
                dllPath = @"/mnt/c/Temp/app/sampleapp/SampleApp.dll";
            }

            var envDict = new Dictionary<string, string>()
            {
                { "FUNCTIONS_WEBAPP_BazWeb","W-BazWeb_FromWebAppDuringProcessStart" },
                { "FUNCTIONS_WEBAPP_FooBar","W-BarFoo_FromWebAppDuringProcessStart" },
                { "FUNCTIONS_APPLICATION_DIRECTORY","W-FuncAppDirFromWebAppDuringProcessStart" }
            };

            StartProcess(path, dir, dllPath, envDict);


            return Enumerable.Range(1, 2).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private void StartProcess(string path, string workingDirectory, string dllPath, IDictionary<string, string> contextEnvironmentVariables)
        {
            var startInfo = new ProcessStartInfo(path)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                ErrorDialog = false,
                WorkingDirectory = workingDirectory,
                Arguments = $"{dllPath}"
            };

            var processEnvVariables = contextEnvironmentVariables;
            if (processEnvVariables.Any())
            {
                foreach (var envVar in processEnvVariables)
                {
                    startInfo.EnvironmentVariables[envVar.Key] = envVar.Value;
                }
            }

            var process = new Process { StartInfo = startInfo };

            try
            {
                process.ErrorDataReceived += (sender, e) => OnErrorDataReceived(sender, e);
                process.OutputDataReceived += (sender, e) => OnOutputDataReceived(sender, e);
                process.Exited += (sender, e) => OnProcessExited(sender, e);
                process.EnableRaisingEvents = true;

                Console.WriteLine($"Starting worker process with FileName:{process.StartInfo.FileName} WorkingDirectory:{process.StartInfo.WorkingDirectory} Arguments:{process.StartInfo.Arguments}");
                process.Start();
                Console.WriteLine($"{process.StartInfo.FileName} process with Id={process.Id} started");

                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start Worker Channel. Process fileName: {process.StartInfo.FileName}. {ex}");
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            Console.WriteLine("Process exited");
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine($"Process OnOutputDataReceived:{e.Data}");
            }
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine($"Process OnErrorDataReceived:{e.Data}");
            }
        }
    }
}