namespace AppLibrary
{
    public class Initializer
    {
        public static void Init()
        {

#if NET5_0_OR_GREATER

            var nativeHostData = NativeMethods.GetNativeHostData();
#else
            Logger.LogInfo("Not supported in current TFM");
#endif

        }
    }
}
