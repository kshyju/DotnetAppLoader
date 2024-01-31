# Set the paths to your project directories
$appLoaderProjectPath = ".\DotnetAppLoader"

# Set the output directory
$outputDirectory = "HostWebApp"

dotnet clean "DotnetAppLoader.sln"

Write-Output "    Publishing DotnetAppLoader project..."
dotnet publish $appLoaderProjectPath -c Release -r win-x64 -o $outputDirectory


#Start-Process $appLoaderExecutable
