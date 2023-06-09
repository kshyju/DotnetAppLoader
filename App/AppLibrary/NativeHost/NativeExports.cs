﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using AppLibrary;

namespace AppLibrary
{
    public struct NativeHostData
    {
        public IntPtr PNativeApplication;
    }

    public static class NativeExports
    {
        [UnmanagedCallersOnly(EntryPoint = "get_application_properties")]
        public static int GetApplicationProperties(NativeHostData nativeHostData)
        {
            try
            {
                var nativeHostApplication = NativeHostApplication.Instance;
                GCHandle gch = GCHandle.Alloc(nativeHostApplication, GCHandleType.Pinned);
                IntPtr pNativeApplication = gch.AddrOfPinnedObject();
                nativeHostData.PNativeApplication = pNativeApplication;

                return 1;
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

            try
            {
                //NativeHostApplication.Instance.SetCallbackHandles(requestCallback, grpcHandler);
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