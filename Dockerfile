# Use the official .NET Framework SDK image
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY . .
RUN nuget restore

# Build the application
RUN msbuild /p:Configuration=Release

# Use a smaller runtime image
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8 AS runtime
WORKDIR /app
COPY --from=build /app/bin/Release .

# The command to run the application
ENTRYPOINT ["MyLibraryProject.exe"]