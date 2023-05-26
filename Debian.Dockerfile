# Build Stage
FROM debian:11 AS build-env

# Install necessary dependencies

RUN apt-get update \
    && apt-get install -y apt-transport-https wget libunwind8 libc6 libgcc-s1 libgssapi-krb5-2 libicu67 libssl1.1 libstdc++6 zlib1g \
    && rm -rf /var/lib/apt/lists/*

RUN apt-get update && apt-get install -y apt-transport-https wget && \
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
RUN dotnet publish -c Release -o out --no-self-contained

# Do the same for DotnetAppLoader
WORKDIR /src/dotnetapploader

# Copy DotnetAppLoader to the working directory
COPY ./DotnetAppLoader ./

# Publish the application. This will do AOT publishing (enabled on project file)
RUN dotnet publish -c Release -o out -p:FUNCTIONS_CONSTANTS=LINUX

# Runtime Stage
FROM debian:11 AS runtime-env

RUN apt-get update \
    && apt-get install -y libc6 libunwind8 libicu67


# Install necessary dependencies
RUN apt-get update \
    && apt-get install -y apt-transport-https \
    && rm -rf /var/lib/apt/lists/*

RUN apt-get update && apt-get install -y wget

RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb 

RUN apt-get update && \
    apt-get install -y aspnetcore-runtime-7.0

# Dependencies listed in https://learn.microsoft.com/en-us/dotnet/core/install/linux-debian#dependencies
RUN apt-get update \
    && apt-get install -y libc6 libgcc-s1 libgssapi-krb5-2 libicu67 libssl1.1 libstdc++6 zlib1g

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

# Prints the search path.
ENV LD_DEBUG=libs

# Keep the app running so that we can inspect needed things in the container.Will not call the DllImport code path.
#ENV DONTCRASH =1

#  Update the system's dynamic linker cache
RUN ldconfig

# Run the FunctionsNetHost application, pass the assembly name (SampleApp.dll) as the argument
ENTRYPOINT ["./FunctionsNetHost", "./../sampleapp/SampleApp.dll"]
