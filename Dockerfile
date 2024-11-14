# Use the official .NET 9 SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

RUN apt-get update && \
    apt-get install -y clang zlib1g-dev

# Set the working directory
WORKDIR /app

# Copy the project files to the container
COPY ./App ./src/App
COPY ./DotnetAppLoader ./src/DotnetAppLoader

# Restore the dependencies
RUN dotnet restore ./src/DotnetAppLoader/DotnetAppLoader.csproj

# Publish the application
RUN dotnet publish ./src/DotnetAppLoader/DotnetAppLoader.csproj -c Release -r linux-x64 -o out

# Build the application
RUN dotnet restore ./src/App/SampleApp/SampleApp.csproj
RUN dotnet publish ./src/App/SampleApp/SampleApp.csproj -c Release -r linux-x64 -o out/sampleapp

CMD [ "./out/FunctionsNetHost","./out/sampleapp/SampleApp.dll" ]
