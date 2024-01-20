using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HostWebApp.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    { 
        Microsoft.Extensions.Hosting.IHostApplicationLifetime _hostApplicationLifetime;

        public HomeController(IHostApplicationLifetime appLifetime)
        {
            _hostApplicationLifetime = appLifetime;
        }   

        [HttpGet]
        public Task<IActionResult> GetAsync()
        {
            var appLoaderExePath = "./DotnetAppLoader/FunctionsNetHost.exe";

            if (!System.IO.File.Exists(appLoaderExePath))
            {
                return Task.FromResult<IActionResult>(Ok("1) Build the project using .\\build\\build_and_run.ps1 from repo root. 2) Execute dotnet dotnet HostWebApp.dll from output . 3)Send GET request to http://localhost:5000"));
            }
            
            _ = Task.Factory.StartNew(() => StartDotnetAppLoaderChildProcess(appLoaderExePath));

            return Task.FromResult<IActionResult>(Ok("Hello from HostWebApp. Check the terminal"));
        }

        private async Task StartDotnetAppLoaderChildProcess(string executablePath)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = executablePath, 
                    UseShellExecute = false,   
                    RedirectStandardOutput = true, 
                    CreateNoWindow = true,   
                    Arguments = "../../SampleApp/SampleApp.dll"
                };

                using (var process = new Process())
                {
                    process.StartInfo = startInfo;
                    _hostApplicationLifetime.ApplicationStopping.Register(() =>
                    {
                        Console.WriteLine("ApplicationStopping fired. Will kill child process");
                        process.Kill();
                    });

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            Console.WriteLine(e.Data); 
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            Console.WriteLine($"[Error] {e.Data}"); 
                        }
                    };

                    var started = process.Start();                    
                    Console.WriteLine($"Started {executablePath} started: {started}");

                    process.BeginOutputReadLine();
                    await process.WaitForExitAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
