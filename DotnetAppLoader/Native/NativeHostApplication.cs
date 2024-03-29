﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotnetAppLoader;

namespace FunctionsNetHost
{
    public sealed class NativeHostApplication
    {
        private IntPtr _workerHandle;
        unsafe delegate* unmanaged<byte**, int, IntPtr, IntPtr> _requestHandlerCallback;
        public static NativeHostApplication Instance { get; } = new();

        private NativeHostApplication()
        {
        }

        public unsafe void SetCallbackHandles(delegate* unmanaged<byte**, int, IntPtr, IntPtr> callback, IntPtr grpcHandle)
        {
            _requestHandlerCallback = callback;
            _workerHandle = grpcHandle;

            Logger.LogInfo("SetCallbackHandles invoked");
        }
    }
}
