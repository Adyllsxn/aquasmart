FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copia tudo (IMPORTANTE para projetos modulares)
COPY . .

WORKDIR /src/src/apps/Aquasmart.Server

RUN dotnet restore
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Aquasmart.Server.dll"]