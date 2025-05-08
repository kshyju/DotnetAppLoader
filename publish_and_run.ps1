# Kill all running DotnetAppLoader and HostWebApp processes before publishing to avoid file lock issues
Get-Process DotnetAppLoader -ErrorAction SilentlyContinue | Stop-Process -Force
Get-Process HostWebApp -ErrorAction SilentlyContinue | Stop-Process -Force

# Define project paths
$solution = "DotnetAppLoader.sln"
$projects = @(
    @{ Name = "HostWebApp"; Path = ".\HostWebApp\HostWebApp.csproj" },
    @{ Name = "DotnetAppLoader"; Path = ".\DotnetAppLoader\DotnetAppLoader.csproj" },
    @{ Name = "ConsoleApp1"; Path = ".\App\ConsoleApp1\ConsoleApp1.csproj" }
)

# Clean solution
Write-Output "Cleaning solution..."
dotnet clean $solution

# Publish all projects
foreach ($proj in $projects) {
    Write-Output "................."
    Write-Output "Publishing $($proj.Name) project..."
    dotnet publish $proj.Path -c Release -r win-x64 
}

# Start the HostWebApp
$hostWebAppExe = ".\out\publish\HostWebApp\release_win-x64\HostWebApp.exe"
Write-Output "Starting HostWebApp: $hostWebAppExe"
# Start-Process $hostWebAppExe