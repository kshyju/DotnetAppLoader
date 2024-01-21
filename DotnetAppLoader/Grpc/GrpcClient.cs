// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Channels;
using DotnetAppLoader;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Azure.Functions.WorkerHarness.Grpc.Messages;
using static Microsoft.Azure.Functions.WorkerHarness.Grpc.Messages.FunctionRpc;

namespace FunctionsNetHost.Grpc
{
    internal sealed class GrpcClient
    {
        private readonly Channel<StreamingMessage> _outgoingMessageChannel;
        string _grpcEndpoint;
        private AppLoader _appLoader;
        string _customerAssemblyPath;
        internal GrpcClient(string endpoint)
        {
            _grpcEndpoint = endpoint;
        }

        public GrpcClient(AppLoader appLoader, string customerAssemblyPath, string grpcEndpoint)
        {
            this._appLoader = appLoader;
            this._customerAssemblyPath = customerAssemblyPath;
            _grpcEndpoint = grpcEndpoint;
        }

        internal async Task InitAsync()
        {
            if (string.IsNullOrEmpty(_grpcEndpoint))
            {
                Logger.LogInfo($"Grpc service endpoint:{_grpcEndpoint}");

                var functionRpcClient = CreateFunctionRpcClient(_grpcEndpoint);
                var eventStream = functionRpcClient.EventStream();

                await SendStartStreamMessageAsync(eventStream.RequestStream);

                var readerTask = StartReaderAsync(eventStream.ResponseStream);

                await Task.WhenAll(readerTask);
            }
            else
            {
                // Don't start GRPC connection. Just load the assembly.
                _appLoader.RunApplication(_customerAssemblyPath);
                await Task.Delay(TimeSpan.FromSeconds(300));
            }
        }

        private async Task StartReaderAsync(IAsyncStreamReader<StreamingMessage> responseStream)
        {
            while (await responseStream.MoveNext())
            {
                await HandleIncomingMessage((StreamingMessage?)responseStream.Current);
            }
        }

        private Task HandleIncomingMessage(StreamingMessage message)
        {
            Task.Run(() =>
           {
               Logger.LogInfo($"Received message from host.ContentCase:{message.ContentCase}");

               if (message.ContentCase == StreamingMessage.ContentOneofCase.WorkerInitRequest)
               {
                   _appLoader.RunApplication(_customerAssemblyPath);
               }

           });
            return Task.CompletedTask;
        }

        private async Task SendStartStreamMessageAsync(IClientStreamWriter<StreamingMessage> requestStream)
        {
            var startStreamMsg = new StartStream()
            {
                WorkerId = Guid.NewGuid().ToString()
            };

            var startStream = new StreamingMessage()
            {
                StartStream = startStreamMsg
            };

            await requestStream.WriteAsync(startStream);
            Logger.LogInfo($"Sent StartStream message to host.");
        }

        private FunctionRpcClient CreateFunctionRpcClient(string endpoint)
        {
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var grpcUri))
            {
                throw new InvalidOperationException($"The gRPC channel URI '{endpoint}' could not be parsed.");
            }

            var grpcChannel = GrpcChannel.ForAddress(grpcUri, new GrpcChannelOptions()
            {
                MaxReceiveMessageSize = int.MaxValue,
                MaxSendMessageSize = int.MaxValue,
                Credentials = ChannelCredentials.Insecure
            });

            return new FunctionRpcClient(grpcChannel);
        }

    }
}
