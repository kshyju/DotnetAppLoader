### To run

#### Build DotnetAppLoader
```bash
> cd DotnetAppLoader\DotnetAppLoader
> dotnet publish -c debug -r win-x64 -o ..\out\loader\
```
#### Build sample app

```bash
> cd DotnetAppLoader\App\SampleApp
> dotnet publish -c release -r win-x64 -o ..\..\out\app\ --self-contained
```


#### Run the generated native executable.
```bash
> cd DotnetAppLoader\out\loader
> DotnetAppLoader.exe ..\app\SampleApp.dll
```

If everything works as expected we should be seeing the logs emitted from native host and the SampleApp. something like below

```
[DotnetAppLoader] Args: ..\app\SampleApp.dll
[DotnetAppLoader] workerAssemblyPath: ..\app\SampleApp.dll
[DotnetAppLoader] hostfxrFullPath: C:\Program Files\dotnet\host\fxr\8.0.0-rc.2.23479.6\hostfxr.dll
[DotnetAppLoader] hostfxr loaded successfully
[DotnetAppLoader] ..\app\SampleApp.dll file.Exists:True
  [App]  Inside SampleApp Main
  [App]  NET7_0_OR_GREATER
[DotnetAppLoader] NativeExports.GetApplicationProperties method invoked.
  [App]  Printing all EnvironmentVariables starts with FUNCTIONS_
```