# Set the base image to use for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Below is needed to do AOT publishing. Without this, we will get below error in publish step of DotNetAppLoader
# Platform linker ('clang' or 'gcc') not found in PATH. Try installing appropriate package for clang or gcc to resolve the problem
# See: https://github.com/dotnet/dotnet-docker/issues/4129#issuecomment-1273620201
RUN apt update
RUN apt install -y clang zlib1g-dev

# Set the working directory inside the container for sample console app
WORKDIR /src/sampleapp

# Copy the sample console app files to the working directory
COPY ./SampleApp ./

# Publish the sample application
RUN dotnet publish -r linux-x64 -c Release -o out --no-self-contained

# Do same for DotnetAppLoader
WORKDIR /src/dotnetapploader

# Copy the project files to the working directory
COPY ./DotnetAppLoader ./

# Publish the application. This will do AOT publishing (enabled on project file)
RUN dotnet publish -r linux-x64 -c Release -o out -p:FUNCTIONS_CONSTANTS=LINUX


# Set the base image to use for running the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory inside the container
WORKDIR /app/sampleapp

# Copy the published output from the build stage to the runtime stage
COPY --from=build-env /src/sampleapp/out ./


WORKDIR /app/dotnetapploader

# Copy the published output from the build stage to the runtime stage
COPY --from=build-env /src/dotnetapploader/out ./

# Expose the port that the web application listens on
EXPOSE 80

RUN dir -s

# Run the application
ENTRYPOINT ["./FunctionsNetHost", "./../sampleapp/SampleApp.dll"]
