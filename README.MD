### Framework Reference dependency loading minimal repro.

### Windows 

1. Publish DotnetAppLoader project
>dotnet publish -c Release -r win-x64 -o out

2. Open solution file and Build the solution. This will build the placeholder app and customer app.

3. Open a new command prompt in the repo root (where the README is present) and run the following command to load the SampleApp.dll using DotnetAppLoader)

> start-dotnetapploader.bat

##### To see the failure.

To repro, comment out `<FrameworkReference Include="Microsoft.AspNetCore.App" />` in `App\CustomerApp\App.csproj` and run again using above steps.


