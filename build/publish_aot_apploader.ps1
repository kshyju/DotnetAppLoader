# Set the paths to your project directories
$appLoaderProjectPath = ".\DotnetAppLoader"

# Set the output directory
$outputDirectory = ".\out"

dotnet clean "DotnetAppLoader.sln"

Write-Output "    Publishing DotnetAppLoader project..."
dotnet publish $appLoaderProjectPath -c Release -r win-x64 -o $outputDirectory\DotnetAppLoader

$appLoaderExecutable = Join-Path $outputDirectory "DotnetAppLoader\FunctionsNetHost.exe"
Write-Output $appLoaderExecutable

#Start-Process $appLoaderExecutable
