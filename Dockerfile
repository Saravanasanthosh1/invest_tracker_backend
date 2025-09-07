# Use official .NET 8 SDK to build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore & publish
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Render provides PORT env var
ENV ASPNETCORE_URLS=http://+:${PORT}

ENTRYPOINT ["dotnet", "investtracker.dll"]
