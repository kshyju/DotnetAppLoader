using System.Diagnostics;
using System.Reflection;

namespace HostWebApp
{
    public sealed class ProcessStarterHostedService(ILogger<ProcessStarterHostedService> logger, IHostApplicationLifetime appLifetime)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            string dotnetAppLoaderPath = Path.GetFullPath(Path.Combine(
                exeDir, "..", "..", "DotnetAppLoader", "release_win-x64", "DotnetAppLoader.exe"));

            if (!File.Exists(dotnetAppLoaderPath))
            {
                throw new FileNotFoundException(
                    $"Expected binary not found at '{dotnetAppLoaderPath}'. Run 'publish_and_run.ps1' to generate the binaries and run the app.");
            }


            await StartDotnetAppLoaderChildProcess(dotnetAppLoaderPath, stoppingToken);
        }

        private async Task StartDotnetAppLoaderChildProcess(string executablePath, CancellationToken stoppingToken)
        {
            logger.LogInformation($"Starting child process ({executablePath})");
            try
            {
                var grpcEndpoint = "http://localhost:6000";
                logger.LogInformation($"grpcEndpoint {grpcEndpoint}");

                var startInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = $"{grpcEndpoint}"
                };

                using var process = new Process();
                process.StartInfo = startInfo;
                appLifetime.ApplicationStopping.Register(() =>
                {
                    logger.LogInformation("IHostApplicationLifetime.ApplicationStopping fired. Will kill child process");
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
                        logger.LogError($"[Error from child process] {e.Data}");
                    }
                };

                var started = process.Start();
                if (!started)
                {
                    logger.LogError($"Failed to start {executablePath}");
                }
                else
                {
                    logger.LogInformation($"Started {executablePath}");
                }

                process.BeginOutputReadLine();
                await process.WaitForExitAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error: {ex}");
            }
        }
    }
}
