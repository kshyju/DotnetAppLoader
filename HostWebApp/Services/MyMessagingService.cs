
using Grpc.Core;
using Microsoft.Azure.Functions.WorkerHarness.Grpc.Messages;
using System.Threading.Channels;

namespace MovieService.Services
{
    public class MyMessagingService : FunctionRpc.FunctionRpcBase
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

            _logger.LogInformation($"....................................................................................................");
            _logger.LogInformation($"---------------------Event Stream executed----{DateTime.Now}----------------------------------------");

            var a = ReceiveMessage(requestStream, context);
            var b = SendMessages(responseStream);

            await Task.WhenAll(a, b);
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
            // Dispatch and return.
            Task.Run(() => ProcessRequestCoreAsync(request));
            return Task.CompletedTask;
        }

        private async Task ProcessRequestCoreAsync(StreamingMessage request)
        {
            _logger.LogInformation($"---------------------<><><> New Message received from client:{request.RequestId},ContentCase:{request.ContentCase}---{DateTime.Now}");

            if (request.ContentCase == StreamingMessage.ContentOneofCase.StartStream)
            {
                _logger.LogInformation($"---------------------Sending WorkerInitRequest---{DateTime.Now}");
                // now send worker init

                var initRequest = new WorkerInitRequest()
                {
                    HostVersion = "1.1.2",
                    WorkerDirectory = "C:\\Dev\\Temp\\FunctionApp44\\FunctionApp44\\bin\\Debug\\net6.0",
                    FunctionAppDirectory = "c://temp"
                };
                var initStreamingMsg = new StreamingMessage { WorkerInitRequest = initRequest };
                await _outgoingMessageChannel.Writer.WriteAsync(initStreamingMsg);
            }
            else if (request.ContentCase == StreamingMessage.ContentOneofCase.RpcLog)
            {
                _logger.LogInformation($@" ~~~ RPC LOG: [{request.RpcLog.Level}] {request.RpcLog.Message} ~~~");
            }
            else
            {
                _logger.LogInformation($"---------------------Some other msg---{request.ContentCase}");
            }

        }
        private async Task SendMessages(IServerStreamWriter<StreamingMessage> responseStream)
        {
            await foreach (StreamingMessage message in _outgoingMessageChannel.Reader.ReadAllAsync())
            {
               // _logger.LogInformation($"Before sending {message.ContentCase.ToString()}");
                await responseStream.WriteAsync(message);
               // _logger.LogInformation($"---------------------Sent{message.ContentCase.ToString()} Finished");
            }
        }
    }
}
