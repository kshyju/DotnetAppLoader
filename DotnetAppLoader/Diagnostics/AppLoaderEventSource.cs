using System.Diagnostics.Tracing;

namespace DotnetAppLoader.Diagnostics
{
    //<Guid("")> , Guid = "4DDBAE46-606F-4590-9717-9B11C70EBE9B"
    [EventSource(Name = "Microsoft-AzureFunctions-DotnetAppLoader")]
    public sealed class AppLoaderEventSource : EventSource
    {
        [Event(8001)]
        public void HostFxrLoadStart(string hostFxrPath)
        {
            if (IsEnabled())
            {
                WriteEvent(8001, hostFxrPath);
            }
        }

        [Event(8002)]
        public void HostFxrLoadStop()
        {
            if (IsEnabled())
            {
                WriteEvent(8002);
            }
        }

        [Event(8003)]
        public void HostFxrInitializeForDotnetCommandLineStart(string assemblyPath)
        {
            if (IsEnabled())
            {
                WriteEvent(8003, assemblyPath);
            }
        }

        [Event(8004)]
        public void HostFxrInitializeForDotnetCommandLineStop()
        {
            if (IsEnabled())
            {
                WriteEvent(8004);
            }
        }

        [Event(8005)]
        public void HostFxrRunAppStart()
        {
            if (IsEnabled())
            {
                WriteEvent(8005);
            }
        }

        public static readonly AppLoaderEventSource Log = new();
    }
}
