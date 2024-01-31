using System.Diagnostics.Tracing;

namespace DotnetAppLoader.Diagnostics
{
    [EventSource(Name = "Microsoft-AzureFunctions-DotnetAppLoader")]
    internal sealed class AppLoaderEventSource : EventSource
    {
        [Event(1)]
        public void HostGrpcHandshakeStart()
        {
            if (IsEnabled())
            {
                WriteEvent(1);
            }
        }

        [Event(2)]
        public void HostGrpcHandshakeStop()
        {
            if (IsEnabled())
            {
                WriteEvent(2);
            }
        }

        [Event(3)]
        public void HostGrpcWritingLog(string message)
        {
            if (IsEnabled())
            {
                WriteEvent(3, message);
            }
        }

        public static readonly AppLoaderEventSource Log = new();
    }
}
