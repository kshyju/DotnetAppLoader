# Use the official Debian 11 base image
FROM debian:11

# Install necessary dependencies
RUN apt-get update && \
    apt-get install -y curl && \
    apt-get clean

# Download and install .NET 6 SDK
RUN curl -SL --output dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/f2e539b3-73a1-4d62-9d7b-80e7f7795f65/da7a8cc0008d09c73f2c3f2f86b6c15e/dotnet-sdk-6.0.100-preview.7.21379.14-linux-x64.tar.gz && \
    mkdir -p /usr/share/dotnet && \
    tar -zxf dotnet.tar.gz -C /usr/share/dotnet && \
    rm dotnet.tar.gz && \
    ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Download and install .NET 6 runtime
RUN curl -SL --output dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/f2e539b3-73a1-4d62-9d7b-80e7f7795f65/da7a8cc0008d09c73f2c3f2f86b6c15e/aspnetcore-runtime-6.0.0-preview.7.21379.14-linux-x64.tar.gz && \
    mkdir -p /usr/share/dotnet && \
    tar -zxf dotnet.tar.gz -C /usr/share/dotnet && \
    rm dotnet.tar.gz

# Set the PATH environment variable
ENV PATH="/usr/share/dotnet:${PATH}"

# Set the working directory inside the container for sample console app
WORKDIR /src/sampleapp

# Copy the sample console app files to the working directory
COPY ./SampleApp ./

RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o out

# Do same for DotnetAppLoader
WORKDIR /src/dotnetapploader

# Copy the project files to the working directory
COPY ./DotnetAppLoader ./

RUN dotnet restore

# Publish the application. This will do AOT publishing (enabled on project file)
RUN dotnet publish -c Release -o out

# Set the working directory inside the container
WORKDIR /app/sampleapp

# Copy the published output from the build stage to the runtime stage
COPY --from=build-env /src/sampleapp/out ./


WORKDIR /app/dotnetapploader

# Copy the published output from the build stage to the runtime stage
COPY --from=build-env /src/dotnetapploader/out ./

# Build and run your application
#CMD ["dotnet", "run"]
ENTRYPOINT ["./DotnetAppLoader", "./../app/sampleapp/SampleApp.dll"]
