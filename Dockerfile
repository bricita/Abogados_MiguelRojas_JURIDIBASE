# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

COPY Abogados_MiguelRojas_JURIDIBASE/*.csproj .
RUN dotnet restore

COPY Abogados_MiguelRojas_JURIDIBASE/ .
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080
COPY --from=build /out .
ENTRYPOINT ["dotnet", "Abogados_MiguelRojas_JURIDIBASE.dll"]
