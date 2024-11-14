Repro app shows 

>On Unix-like systems, in-process environment modifications made by native libraries aren't seen by managed callers.

source: https://learn.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariables?view=net-8.0


## Steps to reproduce

### Windows 

in-process environment modifications made by native libraries are seen by managed callers.

Build SampleApp


Publish DotnetAppLoader project
>dotnet publish -c Release -r win-x64 -o out

Run DotnetAppLoader native executable and pass the path to the SampleApp.dll
>out\FunctionsNetHost.exe "D:\src\DotnetAppLoader\App\SampleApp\bin\Debug\net9.0\SampleApp.dll"


### Linux

in-process environment modifications made by native libraries are NOT seen by managed callers.


Open a terminal at the directory whrere the docker file is located. Build the docker image.

>docker build -t dotnetapploader:1 -f .\Dockerfile .

Run the container using this docker image.

>docker run dotnetapploader:1

