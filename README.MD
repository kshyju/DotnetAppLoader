

### Testing in Linux

The docker image present in the root can be used to build a container where the issue can be reproduced. To build the image, open a terminal in the repo root (where you will see the `Debian.Dockerfile`). Run the below command to build the image.

```bash
> docker build -t apploader -f .\Debian.Dockerfile .
```
List all images
```bash
> docker images
```
Run the container from the newly created image.

```bash
> docker run -p 8080:80 {imageid}
```

### Docker image for testing Windows

The same code works fine in Windows. The only change we need to make is to udpate the dll name to include .exe suffix when using the `DllImport` attribute.

https://github.com/kshyju/DotnetAppLoader/blob/517dc3dfb91004b23e006815fb3c93570a80d931/SampleApp/NativeMethods.cs#L13

To test, publish both apps. Execute "FunctionsNetHost.exe" and pass the assembly path of the SampleApp publish/build output.

Publishing DotnetAppLoader/FunctionsNetHost

```bash
> dotnet publish -c release -r win-x64 -o C:\Temp\app\dotnetapploader
```

Publishing the sample app to "C:\Temp\app\sampleapp"

```bash
> dotnet publish -c release -r win-x64 -o C:\Temp\app\sampleapp --no-self-contained
```