Repro app which fails to load a x86 assembly.

### Steps to repro:

#### Publish the dotnet app loader (AOT publish to native exe)
1. > cd \DotnetAppLoader\DotnetAppLoader
2. > dotnet publish -r win-x64 

#### Publish the assembly as x86 

1. > cd DotnetAppLoader\App\SampleApp
2. > dotnet publish -c release -r win-x86 --no-self-contained

Run the native exe, pass the assembly path to load

Ex:
> .\FunctionsNetHost.exe C:\src\OSS\DotnetAppLoader\App\SampleApp\bin\release\net6.0\win-x86\SampleApp.dll


### Current behavior:

Getting below exception:

> Unhandled exception. System.BadImageFormatException: Could not load file or assembly 'C:\src\OSS\DotnetAppLoader\App\SampleApp\bin\release\net6.0\win-x86\SampleApp.dll'. An attempt was made to load a program with an incorrect format.

