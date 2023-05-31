namespace AppLibrary
{
    public class Initializer
    {
        public static void Init()
        {
            Logger.LogInfo("Initializing...");

            var nativeHostData = new NativeMethods().GetNativeHostData();
            Logger.LogInfo($"NativeHost Application Ptr: {nativeHostData.pNativeApplication}");
        }
    }
}
