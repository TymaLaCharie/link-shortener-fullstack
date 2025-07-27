# Get the .NET SDK to facilitate the API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG http_proxy
ARG https_proxy

# This sets the proxy environment variables INSIDE the container for this build stage
ENV http_proxy=$http_proxy
ENV https_proxy=$https_proxy

WORKDIR /src

# Copy all project file and restore API packages.
# This command will now use the proxy settings defined above.
COPY ["LinkShortner.Api/LinkShortner.Api.csproj", "LinkShortner.Api/"]
RUN dotnet restore "LinkShortner.Api/LinkShortner.Api.csproj"

# Copy all of the contents of the API
COPY . .
WORKDIR "/src/LinkShortner.Api"
# Build the application for release
RUN dotnet build "LinkShortner.Api.csproj" -c Release -o /app/build

# Publishing the API
FROM build AS publish
RUN dotnet publish "LinkShortner.Api.csproj" -c Release -o /app/publish

# Create a container to run the API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published artifacts
COPY --from=publish /app/publish .
# Command to run the API when the container starts
ENTRYPOINT ["dotnet", "LinkShortner.Api.dll"]