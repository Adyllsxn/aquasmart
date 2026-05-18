FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["src/apps/Aquasmart.Web/Aquasmart.Web.csproj", "src/apps/Aquasmart.Web/"]
COPY ["src/orchestration/aspire/Aquasmart.ServiceDefaults/Aquasmart.ServiceDefaults.csproj", "src/orchestration/aspire/Aquasmart.ServiceDefaults/"]

RUN dotnet restore "src/apps/Aquasmart.Web/Aquasmart.Web.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/src/apps/Aquasmart.Web"
RUN dotnet build "Aquasmart.Web.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Aquasmart.Web.csproj" -c Release -o /app/publish

# Stage 3: Runtime (Nginx for Blazor WASM)
FROM nginx:alpine AS runtime
WORKDIR /usr/share/nginx/html

# Copy published files
COPY --from=publish /app/publish/wwwroot .

# Copy nginx config
COPY src/apps/Aquasmart.Web/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/ || exit 1