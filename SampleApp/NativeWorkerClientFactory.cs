namespace SampleApp
{
    internal class NativeWorkerClientFactory
    {

        internal NativeHost CreateClient()
        {
            var nativeHostData = NativeMethods.GetNativeHostData();
            return nativeHostData;
        }
    }
}
