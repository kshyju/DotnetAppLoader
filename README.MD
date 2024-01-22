## Case 1 - NO AOT Publish

### To run

#### 1. Publish DotnetAppLoader 
Open a powershell terminal and run the below commands from the root of the repo.

```bash
> .\build\publish_aot_apploader.ps1
```

This will publish the DotnetAppLoader project to the `out\DotnetAppLoader` folder.
#### 2. Run the host web app.

```bash
> cd HostWebApp
> dotnet run
```

This will start the "HostWebApp" which will start a GRPC server & also start a child process which will launch the published binary of "DotnetAppLoader". The DotnetAppLoader has GRPC client code which will establish a GRPC connection with the GRPC service on HostWebApp.

If everything works as expected we should be seeing the logs emitted like below

```
[DotnetAppLoader] Raw Command line args: http://localhost:5138
[DotnetAppLoader] Grpc service endpoint:http://localhost:5138
[DotnetAppLoader] Sent StartStream message to host.
[DotnetAppLoader] HostFxr loaded successfully. About to call HostFxr.Initialize.
info: FunctionRpcGrpcService.MyMessagingService[0]
      ---------------------<><><> New Message received from client.ContentCase:StartStream---1/21/2024 7:45:13 PM
 [DotnetAppLoader] Received message from host.ContentCase:WorkerInitRequest
info: FunctionRpcGrpcService.MyMessagingService[0]
      ---------------------<><><> New Message received from client.ContentCase:RpcLog---1/21/2024 7:45:13 PM
info: FunctionRpcGrpcService.MyMessagingService[0]
       ~~~ RPC LOG: Hello 1 from AppLoader. ~~~
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5138
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7138
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Dev\OSS\DotnetAppLoader\HostWebApp
info: FunctionRpcGrpcService.MyMessagingService[0]
      ---------------------<><><> New Message received from client.ContentCase:RpcLog---1/21/2024 7:45:15 PM
info: FunctionRpcGrpcService.MyMessagingService[0]
       ~~~ RPC LOG: Hello 2 from AppLoader. ~~~
info: FunctionRpcGrpcService.MyMessagingService[0]
      ---------------------<><><> New Message received from client.ContentCase:RpcLog---1/21/2024 7:45:17 PM
info: FunctionRpcGrpcService.MyMessagingService[0]
       ~~~ RPC LOG: Hello 3 from AppLoader. ~~~
info: FunctionRpcGrpcService.MyMessagingService[0]
```

### To reproduce the issue
 1. Start perfview collect with ".NET" provider enabled. 
 2. Run option 2 above.
 3. Observe the logs emitted.

When "e13c0d23-ccbc-4e12-931b-d9cc2eee27e4" provider is disabled, things will work as expected.
