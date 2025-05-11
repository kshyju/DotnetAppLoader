## Case 1 - AOT Publish

### To run

#### 1. Publish DotnetAppLoader 
Open a powershell terminal and run the below commands from the root of the repo.

```bash
> .\publish_and_run.ps1
```

This will publish all three projects and start the "HostWebApp" which will start a GRPC server & also start a child process which will launch the published binary of "DotnetAppLoader". The DotnetAppLoader has GRPC client code which will establish a GRPC connection with the GRPC service on HostWebApp.
After about 10 seconds, the DotnetAppLoader will load the ConsoleApp1.dll.
