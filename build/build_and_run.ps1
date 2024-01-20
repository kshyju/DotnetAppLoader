# Set the paths to your project directories
$hostWebAppProjectPath = ".\HostWebApp"
$appLoaderProjectPath = ".\DotnetAppLoader"
$customerAppProjectPath = ".\App\SampleApp"

# Set the output directory
$outputDirectory = ".\out"

dotnet publish $hostWebAppProjectPath -c Release -r win-x64 -o $outputDirectory\HostWebApp

Write-Output "    Publishing DotnetAppLoader project..."
dotnet publish $appLoaderProjectPath -c Release -r win-x64 -o $outputDirectory\HostWebApp\DotnetAppLoader

Write-Output "    Publishing SampleApp project..."
dotnet publish $customerAppProjectPath -c Release -r win-x64 -o $outputDirectory\SampleApp

$appLoaderExecutable = Join-Path $outputDirectory "DotnetAppLoader\FunctionsNetHost.exe"
Write-Output $appLoaderExecutable

#Start-Process $appLoaderExecutable
