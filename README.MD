
### To run

#### AOT Publish DotnetAppLoader
Open a powershell terminal and run the below commands from the root of the repo.

```bash
> .\build\publish_aot_apploader.ps1
```

This will publish native executable and the sample .net app assembly to the `out` folder.
#### Run the app

##### Option 1 - Run the native executable using command line.

```bash
> out\DotnetAppLoader
> .\FunctionsNetHost.exe ..\SampleApp\SampleApp.dll
```


##### Option 1 - Run as a child process launched from web app host.

```bash
> cd HostWebApp
> dotnet run
```

If everything works as expected we should be seeing the logs emitted from native host and the SampleApp. something like below

```
[DotnetAppLoader] Raw Command line args: ..\SampleApp\SampleApp.dll
[DotnetAppLoader] file ..\SampleApp\SAmpleApp.dll exists:True
[DotnetAppLoader] HostFxr path: C:\Program Files\dotnet\host\fxr\8.0.1\hostfxr.dll
[DotnetAppLoader] HostFxr loaded successfully. About to call HostFxr.Initialize.
[DotnetAppLoader] About to call HostFxr.Run
  [CustomerApp]   Inside Customer assembly SampleApp Main
  [CustomerApp]   Inside Customer Assembly AppLibrary.NativeClient.Start()
  [CustomerApp]  NET7_0_OR_GREATER
[DotnetAppLoader] NativeExports.RegisterCallbacks method invoked
```

### To reproduce the issue
 1. Start perfview collect with ".NET" provider enabled. 
 2. Run option 2 above.
 3. Observe the logs emitted.

