
using Grpc.Core;
using Microsoft.Azure.Functions.WorkerHarness.Grpc.Messages;
using System.Reflection;
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
                if (request.RpcLog.Message.Contains("Hello 10"))
                {

                    string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
                    string assemlyPath = Path.GetFullPath(Path.Combine(
                        exeDir, "..", "..", "ConsoleApp1", "release_win-x64", "ConsoleApp1.dll"));

                    var path = Path.GetFullPath(assemlyPath);
                    var environmentReloadRequest = new FunctionEnvironmentReloadRequest()
                    {
                        EnvironmentVariables = { { "MY_ENV_VAR1", "MY_ENV_VALUE" } },
                        FunctionAppDirectory = path,
                    };
                    var environmentReloadStreamingMsg = new StreamingMessage { FunctionEnvironmentReloadRequest = environmentReloadRequest };
                    await _outgoingMessageChannel.Writer.WriteAsync(environmentReloadStreamingMsg);

                }
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
