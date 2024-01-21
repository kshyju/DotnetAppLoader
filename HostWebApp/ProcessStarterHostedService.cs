
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HostWebApp
{
    public sealed class ProcessStarterHostedService : BackgroundService
    {
        private const string CustomerAppAssemblyPath = "./../SampleApp/SampleApp.dll";
        private readonly ILogger<ProcessStarterHostedService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProcessStarterHostedService(ILogger<ProcessStarterHostedService> logger, IWebHostEnvironment webHostEnvironment, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = appLifetime;
            _webHostEnvironment = webHostEnvironment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var appLoaderExePath = Path.GetFullPath(Path.Combine(_webHostEnvironment.ContentRootPath, "./DotnetAppLoader/FunctionsNetHost.exe"));

            if (!File.Exists(appLoaderExePath))
            {
                _logger.LogWarning(
                    "1) Build the project using .\\build\\build_and_run.ps1 from repo root. " +
                    "2) Execute dotnet dotnet HostWebApp.dll from output.");
            }

            await StartDotnetAppLoaderChildProcess(appLoaderExePath, stoppingToken);
        }

        private async Task StartDotnetAppLoaderChildProcess(string executablePath, CancellationToken stoppingToken)
        {
            var customerAssemblyPath = Path.GetFullPath(Path.Combine(_webHostEnvironment.ContentRootPath, CustomerAppAssemblyPath));
            _logger.LogInformation($"Starting child process ({executablePath}) which will load .NET assembly ({customerAssemblyPath}");
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = customerAssemblyPath
                };

                using (var process = new Process())
                {
                    process.StartInfo = startInfo;
                    _hostApplicationLifetime.ApplicationStopping.Register(() =>
                    {
                        _logger.LogInformation("IHostApplicationLifetime.ApplicationStopping fired. Will kill child process");
                        process.Kill();
                    });

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            Console.WriteLine(" " + e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            _logger.LogError($"[Error from child process] {e.Data}");
                        }
                    };

                    HostWebAppEventSource.Log.ChildProcessStart(executablePath);

                    var started = process.Start();
                    if (!started)
                    {
                        _logger.LogError($"Failed to start {executablePath}");
                    }

                    process.BeginOutputReadLine();
                    await process.WaitForExitAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.ToString()}");
            }
        }
    }
}
