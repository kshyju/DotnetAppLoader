using System.Diagnostics.Tracing;

namespace HostWebApp
{
    [EventSource(Name = "Microsoft-AzureFunctions-Host")]
    public sealed class HostWebAppEventSource : EventSource
    {
        [Event(6001)]
        public void ChildProcessStart(string executablePath)
        {
            if (IsEnabled())
            {
                WriteEvent(6001, executablePath);
            }
        }

        public static readonly HostWebAppEventSource Log = new();
    }
}
