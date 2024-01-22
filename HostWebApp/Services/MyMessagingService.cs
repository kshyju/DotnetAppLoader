
using Grpc.Core;
using Microsoft.Azure.Functions.WorkerHarness.Grpc.Messages;
using System.Threading.Channels;

namespace FunctionRpcGrpcService
{
    public sealed class MyMessagingService : FunctionRpc.FunctionRpcBase
    {
        private readonly Channel<StreamingMessage> _outgoingMessageChannel;
        private readonly ILogger<MyMessagingService> _logger;
        public MyMessagingService(ILogger<MyMessagingService> logger)
        {
            _logger = logger;

            var outputOptions = new UnboundedChannelOptions
            {
                SingleWriter = false,
                SingleReader = false,
                AllowSynchronousContinuations = false
            };
            _outgoingMessageChannel = Channel.CreateUnbounded<StreamingMessage>(outputOptions);
        }

        public override async Task EventStream(IAsyncStreamReader<StreamingMessage> requestStream,
            IServerStreamWriter<StreamingMessage> responseStream, ServerCallContext context)
        {

            var receiveTask = ReceiveMessage(requestStream, context);
            var sendTask = SendMessages(responseStream);

            await Task.WhenAll(receiveTask, sendTask);
        }

        private async Task ReceiveMessage(IAsyncStreamReader<StreamingMessage> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                await ProcessRequestAsync(requestStream.Current);
            }
        }
        private Task ProcessRequestAsync(StreamingMessage request)
        {
            Task.Run(() => ProcessRequestCoreAsync(request));
            return Task.CompletedTask;
        }

        private async Task ProcessRequestCoreAsync(StreamingMessage request)
        {
            _logger.LogInformation($"---------------------<><><> New Message received from client.ContentCase:{request.ContentCase}---{DateTime.Now}");

            if (request.ContentCase == StreamingMessage.ContentOneofCase.StartStream)
            {
                var initRequest = new WorkerInitRequest()
                {
                };
                var initStreamingMsg = new StreamingMessage { WorkerInitRequest = initRequest };
                await _outgoingMessageChannel.Writer.WriteAsync(initStreamingMsg);
            }
            else if (request.ContentCase == StreamingMessage.ContentOneofCase.RpcLog)
            {
                _logger.LogInformation($@" ~~~ RPC LOG: {request.RpcLog.Message} ~~~");
            }
        }
        private async Task SendMessages(IServerStreamWriter<StreamingMessage> responseStream)
        {
            await foreach (StreamingMessage message in _outgoingMessageChannel.Reader.ReadAllAsync())
            {
                await responseStream.WriteAsync(message);
            }
        }
    }
}
