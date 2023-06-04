namespace AppLibrary
{
    public class Initializer
    {
        public static void Init()
        {
            Logger.LogInfo("Initializing...");

#if NET5_0_OR_GREATER

            var nativeHostData = NativeMethods.GetNativeHostData();
            Logger.LogInfo($"NativeHost Application Ptr: {nativeHostData.pNativeApplication}");
#else
            Logger.LogInfo("Not supported in current TFM");
#endif

            Logger.LogInfo("Init done.");
        }
    }
}
