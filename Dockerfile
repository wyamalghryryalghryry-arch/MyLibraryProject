# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the application files
COPY . .
# Publish the application
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image for the final application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# The command to run the application
ENTRYPOINT ["dotnet", "MyLibraryProject.dll"]