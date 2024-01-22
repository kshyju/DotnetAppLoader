// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Channels;
using DotnetAppLoader;
using DotnetAppLoader.Diagnostics;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Azure.Functions.WorkerHarness.Grpc.Messages;
using static Microsoft.Azure.Functions.WorkerHarness.Grpc.Messages.FunctionRpc;

namespace FunctionsNetHost.Grpc
{
    internal sealed class GrpcClient
    {
        private readonly Channel<StreamingMessage> _outgoingMessageChannel;
        private readonly string _grpcEndpoint;

        internal GrpcClient(string endpoint)
        {
            _grpcEndpoint = endpoint;
            var channelOptions = new UnboundedChannelOptions
            {
                SingleWriter = false,
                SingleReader = false,
                AllowSynchronousContinuations = true
            };

            _outgoingMessageChannel = Channel.CreateUnbounded<StreamingMessage>(channelOptions);
        }

        internal async Task InitAsync()
        {
            Logger.LogInfo($"Grpc service endpoint:{_grpcEndpoint}");

            var functionRpcClient = CreateFunctionRpcClient(_grpcEndpoint);
            var eventStream = functionRpcClient.EventStream();

            var readerTask = StartReaderAsync(eventStream.ResponseStream);
            var writerTask = StartWriterAsync(eventStream.RequestStream);

            await SendStartStreamMessageAsync(eventStream.RequestStream);

            await Task.WhenAll(readerTask, writerTask);
        }

        private async Task StartReaderAsync(IAsyncStreamReader<StreamingMessage> responseStream)
        {
            while (await responseStream.MoveNext())
            {
                var msg = responseStream.Current;
                _ = Task.Run(() => HandleIncomingMessage(msg));
            }
        }

        private async Task StartWriterAsync(IClientStreamWriter<StreamingMessage> requestStream)
        {
            await foreach (var rpcWriteMsg in _outgoingMessageChannel.Reader.ReadAllAsync())
            {
                await requestStream.WriteAsync(rpcWriteMsg);
            }
        }

        private async Task HandleIncomingMessage(StreamingMessage message)
        {
            Logger.LogInfo($"Received message from host.ContentCase:{message.ContentCase}");

            if (message.ContentCase == StreamingMessage.ContentOneofCase.WorkerInitRequest)
            {
                AppLoaderEventSource.Log.HostGrpcHandshakeStop();

                // Send some Log messages back to host.
                for (var i = 0; i < 10; i++)
                {
                    var streamingMessage = new StreamingMessage()
                    {
                        RpcLog = new RpcLog()
                        {
                            LogCategory = RpcLog.Types.RpcLogCategory.System,
                            Message = $"Hello {i} from AppLoader."
                        }
                    };

                    await _outgoingMessageChannel.Writer.WriteAsync(streamingMessage);
                    await Task.Delay(1000);
                }
            }
        }

        private async Task SendStartStreamMessageAsync(IClientStreamWriter<StreamingMessage> requestStream)
        {
            AppLoaderEventSource.Log.HostGrpcHandshakeStart();

            var startStream = new StreamingMessage()
            {
                StartStream = new StartStream()
                {
                    WorkerId = Guid.NewGuid().ToString()
                }
            };

            await _outgoingMessageChannel.Writer.WriteAsync(startStream);
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