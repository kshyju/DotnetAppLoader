using System.Diagnostics;

namespace HostWebApp
{
    public sealed class ProcessStarterHostedService : BackgroundService
    {
        private readonly ILogger<ProcessStarterHostedService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public ProcessStarterHostedService(
            ILogger<ProcessStarterHostedService> logger,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, 
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = appLifetime;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Give the GRPC server time to startup
            await Task.Delay(2000);

            var appLoaderExePath = Path.GetFullPath(Path.Combine(_webHostEnvironment.ContentRootPath, "../out/DotnetAppLoader/FunctionsNetHost.exe"));
            if (!File.Exists(appLoaderExePath))
            {
                _logger.LogWarning("Run ./build/publish_aot.apploader.ps1 first");
            }

            if (!File.Exists(appLoaderExePath))
            {
                _logger.LogError($"Could not find {appLoaderExePath}");
            }

            var customerAssemblyPath = Path.GetFullPath(Path.Combine(_webHostEnvironment.ContentRootPath, "./../out/SampleApp/SampleApp.dll"));
            if (!File.Exists(customerAssemblyPath))
            {
                _logger.LogWarning("Missing customer assembly file. Run ./build/publish_aot.apploader.ps1 first");
            }
            if (!File.Exists(customerAssemblyPath))
            {
                _logger.LogError($"Could not find {customerAssemblyPath}");
            }

             await StartDotnetAppLoaderChildProcess(appLoaderExePath, customerAssemblyPath, stoppingToken);
        }

        private async Task StartDotnetAppLoaderChildProcess(string executablePath, string customerAssemblyPath, CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting child process ({executablePath}) which will load .NET assembly ({customerAssemblyPath}");
            try
            {
                var grpcEndpoint = _configuration["urls"].Split(";")[0];

                var startInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = $"{customerAssemblyPath} {grpcEndpoint}"
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

                    //HostWebAppEventSource.Log.ChildProcessStart(executablePath);

                    var started = process.Start();
                    if (!started)
                    {
                        _logger.LogError($"Failed to start {executablePath}");
                    }
                    else
                    {
                        _logger.LogInformation($"Started {executablePath}");
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
