### Framework Reference dependency loading minimal repro.

### Windows 

1. Publish DotnetAppLoader project. Open a command prompt in the repo root(where the README is present) and run below commands.

 ```bash
>cd DotnetAppLoader
>dotnet publish -c Release -r win-x64 -o out
``` 
2. Open solution file in Visual Studio and Build the solution. This will build the placeholder app and customer app.
3. Open a new command prompt in the repo root and run the following commands. Make sure you update the path to the PlaceholderApp.dll in the below command to reflect your local location.
 ```bash

```bash
>cd DotnetAppLoader\out
>set DOTNET_STARTUP_HOOKS=D:\src\DotnetAppLoader\App\PlaceholderApp\bin\Debug\net9.0\PlaceholderApp.dll
>start-dotnetapploader.bat
``` 
This will start the native process and start Placeholder.dll(managed code). The startuphook code will use `Assembly.SetEntryAssembly` API to change the entry assembly to App.dll. We should be seeing console log entries with prefix `[CustomerApp]`

##### To see the failure.

To repro the issue, uncomment out `<FrameworkReference Include="Microsoft.AspNetCore.App" />` in `App\CustomerApp\App.csproj` and run again using above steps(from step 2).
We should see the below error.
```
Unhandled exception. System.IO.FileNotFoundException: Could not load file or assembly 'Microsoft.Extensions.Hosting.Abstractions, Version=9.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'. The system cannot find the file specified.
File name: 'Microsoft.Extensions.Hosting.Abstractions, Version=9.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'
   at SampleApp.Program.Main(String[] args)
   at System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start[TStateMachine](TStateMachine& stateMachine)
   at SampleApp.Program.Main(String[] args)
   at SampleApp.Program.<Main>(String[] args)
   at System.RuntimeMethodHandle.InvokeMethod(Object target, Void** arguments, Signature sig, Boolean isConstructor)
   at System.Reflection.MethodBaseInvoker.InvokeDirectByRefWithFewArgs(Object obj, Span`1 copyOfArgs, BindingFlags invokeAttr)
   at System.Reflection.MethodBaseInvoker.InvokeWithOneArg(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)
   at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)
   at System.AppDomain.ExecuteAssembly(Assembly assembly, String[] args)
   at FunctionsNetHost.Program.Main(String[] args) in D:\src\DotnetAppLoader\App\PlaceholderApp\Program.cs:line 15
```

