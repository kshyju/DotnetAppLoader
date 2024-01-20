# Set the paths to your project directories
$appLoaderProjectPath = "..\DotnetAppLoader"
$customerAppProjectPath = "..\App\SampleApp"

# Set the output directory
$outputDirectory = "..\out"

Write-Output "    Publishing DotnetAppLoader project..."
dotnet publish $appLoaderProjectPath -c Release -r win-x64 -o $outputDirectory\DotnetAppLoader

Write-Output "    Publishing SampleApp project..."
dotnet publish $customerAppProjectPath -c Release -r win-x64 -o $outputDirectory\SampleApp

$appLoaderExecutable = Join-Path $outputDirectory "DotnetAppLoader\FunctionsNetHost.exe"
Write-Output $appLoaderExecutable

#Start-Process $appLoaderExecutable
