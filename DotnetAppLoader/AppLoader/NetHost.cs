using System.Runtime.InteropServices;

namespace DotnetAppLoader
{
    internal class NetHost
    {
        public unsafe struct get_hostfxr_parameters
        {
            public nint size;
            public char* assembly_path;
            public char* dotnet_root;
        }

        [DllImport("nethost", CharSet = CharSet.Auto)]
        private unsafe static extern int get_hostfxr_path(
        [Out] char[] buffer,
        [In] ref int buffer_size,
        get_hostfxr_parameters* parameters);

        internal unsafe static string GetHostFxrPath(get_hostfxr_parameters* parameters)
        {
            char[] buffer = new char[200];
            int buffer_size = buffer.Length;

            int rc = get_hostfxr_path(buffer, ref buffer_size, parameters);

            if (rc != 0)
            {
                throw new InvalidOperationException("Failed to get the hostfxr path.");
            }

            return new string(buffer, 0, buffer_size - 1);
        }
    }
}
