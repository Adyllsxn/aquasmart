FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/apps/Aquasmart.Server/Aquasmart.Server.csproj", "src/apps/Aquasmart.Server/"]
COPY ["src/orchestration/aspire/Aquasmart.ServiceDefaults/Aquasmart.ServiceDefaults.csproj", "src/orchestration/aspire/Aquasmart.ServiceDefaults/"]

RUN dotnet restore "src/apps/Aquasmart.Server/Aquasmart.Server.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/apps/Aquasmart.Server"
RUN dotnet build "Aquasmart.Server.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Aquasmart.Server.csproj" -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Aquasmart.Server.dll"]