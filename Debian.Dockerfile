# Build Stage
#FROM debian:11 AS build-env
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Below is needed to do AOT publishing. Without this, we will get below error in publish step of DotNetAppLoader
# Platform linker ('clang' or 'gcc') not found in PATH. Try installing appropriate package for clang or gcc to resolve the problem
# See: https://github.com/dotnet/dotnet-docker/issues/4129#issuecomment-1273620201
RUN apt update
RUN apt install -y clang zlib1g-dev

# Set the PATH environment variable for dotnet
ENV PATH="/usr/share/dotnet:${PATH}"

#  Update the system's dynamic linker cache
# RUN ldconfig

# Set the working directory inside the container for the sample app
WORKDIR /src/sampleapp

# Copy the sample console app files to the working directory
COPY ./SampleApp ./

# Publish the sample application to "out" directory
RUN dotnet publish -r linux-x64 -c Release -o out --no-self-contained

# Do the same for DotnetAppLoader
WORKDIR /src/dotnetapploader

# Copy DotnetAppLoader to the working directory
COPY ./DotnetAppLoader ./

# Publish the application. This will do AOT publishing (enabled on project file)
RUN dotnet publish -r linux-x64 -c Release -o out -p:FUNCTIONS_CONSTANTS=LINUX



# Runtime Stage
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS runtime-env

RUN apt-get update && apt-get install -y clang libnuma1

# Debian 11 .NET 7 dependencies as per https://github.com/dotnet/core/blob/main/release-notes/7.0/linux-packages.md#debian-11-bullseye
# Also see https://learn.microsoft.com/en-us/dotnet/core/install/linux-debian#dependencies


# Set the PATH environment variable for dotnet
ENV PATH="/usr/share/dotnet:${PATH}"

# Set the working directory inside the container
WORKDIR /app/sampleapp

# Copy the published output from the build stage to the runtime stage
COPY --from=build-env /src/sampleapp/out ./

# Set the working directory for DotnetAppLoader
WORKDIR /app/dotnetapploader

# Copy the published output from the build stage to the runtime stage
COPY --from=build-env /src/dotnetapploader/out ./

# Expose the port that the web application listens on
EXPOSE 80

# Set the LD_LIBRARY_PATH environment variable
ENV LD_LIBRARY_PATH="/app/dotnetapploader"

# Prints the search path.
#ENV LD_DEBUG=libs

# Keep the app running so that we can inspect needed things in the container.Will not call the DllImport code path.
#ENV DONTCRASH =1

#  Update the system's dynamic linker cache
#RUN ldconfig

# Run the FunctionsNetHost application, pass the assembly name (SampleApp.dll) as the argument
ENTRYPOINT ["./FunctionsNetHost", "./../sampleapp/SampleApp.dll"]