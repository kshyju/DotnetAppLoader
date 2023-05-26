# Build Stage
FROM debian:11 AS build-env

# Install necessary dependencies
RUN apt-get update \
    && apt-get install -y apt-transport-https wget \
    && rm -rf /var/lib/apt/lists/*

RUN apt-get update && apt-get install -y apt-transport-https wget clang clang zlib1g-dev && \
    wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-7.0 && \
    rm -rf /var/lib/apt/lists/*    

# Download and install the Microsoft package repository for .NET
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb

# Update the package list and install .NET 7 SDK
RUN apt-get update \
    && apt-get install -y dotnet-sdk-7.0 \
    && rm -rf /var/lib/apt/lists/*

# Set the PATH environment variable for dotnet
ENV PATH="/usr/share/dotnet:${PATH}"

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
FROM debian:11 AS runtime-env

# Install necessary dependencies
RUN apt-get update \
    && apt-get install -y apt-transport-https libicu-dev \
    && rm -rf /var/lib/apt/lists/*

RUN apt-get update && apt-get install -y wget

# # Download and install the Microsoft package repository for .NET
# RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
#     && dpkg -i packages-microsoft-prod.deb \
#     && rm packages-microsoft-prod.deb

# # Update the package list and install .NET 7 runtime
# RUN apt-get update \
#     && apt-get install -y dotnet-runtime-7.0 \
#     && rm -rf /var/lib/apt/lists/*

# # Set the PATH environment variable for dotnet
# ENV PATH="/usr/share/dotnet:${PATH}"

# pulling in full sDK for testing now. Can replace to runtime only later.
RUN apt-get update && apt-get install -y apt-transport-https wget clang clang zlib1g-dev && \
    wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-7.0 && \
    rm -rf /var/lib/apt/lists/*    

# Download and install the Microsoft package repository for .NET
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb

# Update the package list and install .NET 7 SDK
RUN apt-get update \
    && apt-get install -y dotnet-sdk-7.0 \
    && rm -rf /var/lib/apt/lists/*

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
# ENV LD_LIBRARY_PATH="/app/dotnetapploader"

#  Update the system's dynamic linker cache
RUN ldconfig

# Run the FunctionsNetHost application, pass the assembly name (SampleApp.dll) as the argument
ENTRYPOINT ["./FunctionsNetHost", "./../sampleapp/SampleApp.dll"]
