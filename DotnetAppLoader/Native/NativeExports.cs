﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using DotnetAppLoader;

namespace FunctionsNetHost
{
    public static class NativeExports
    {
        [UnmanagedCallersOnly(EntryPoint = "get_application_properties")]
        public static int GetApplicationProperties(NativeHostData nativeHostData)
        {
            Logger.LogDebug("NativeExports.GetApplicationProperties method invoked.");
            
            try
            {
                var nativeHostApplication = NativeHostApplication.Instance;
                GCHandle gch = GCHandle.Alloc(nativeHostApplication, GCHandleType.Pinned);
                IntPtr pNativeApplication = gch.AddrOfPinnedObject();
                nativeHostData.PNativeApplication = pNativeApplication;

                return 101;
            }
            catch (Exception ex)
            {
                Logger.LogInfo($"Error in NativeExports.GetApplicationProperties: {ex}");
                return 0; 
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "register_callbacks")]
        public static unsafe int RegisterCallbacks(IntPtr pInProcessApplication,
                                                delegate* unmanaged<byte**, int, IntPtr, IntPtr> requestCallback,
            IntPtr grpcHandler)
        {
            Logger.LogDebug("NativeExports.RegisterCallbacks method invoked.");
            
            try
            {
                NativeHostApplication.Instance.SetCallbackHandles(requestCallback, grpcHandler);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.LogInfo($"Error in RegisterCallbacks: {ex}");
                return 0;
            }
        }
    }
}
