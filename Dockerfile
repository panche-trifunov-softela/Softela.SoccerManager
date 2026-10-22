# Image for the SoccerManager API.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Project files first, so the restore layer is reused until a dependency changes.
COPY SoccerManager.API/SoccerManager.API.csproj SoccerManager.API/
COPY SoccerManager.Application/SoccerManager.Application.csproj SoccerManager.Application/
COPY SoccerManager.Domain/SoccerManager.Domain.csproj SoccerManager.Domain/
COPY SoccerManager.Infrastructure/SoccerManager.Infrastructure.csproj SoccerManager.Infrastructure/
RUN dotnet restore SoccerManager.API/SoccerManager.API.csproj

COPY SoccerManager.API/ SoccerManager.API/
COPY SoccerManager.Application/ SoccerManager.Application/
COPY SoccerManager.Domain/ SoccerManager.Domain/
COPY SoccerManager.Infrastructure/ SoccerManager.Infrastructure/

# The migration scripts publish next to SoccerManager.Infrastructure.dll, which is
# where the migrator looks for them.
RUN dotnet publish SoccerManager.API/SoccerManager.API.csproj -c Release -o /app --no-restore -p:UseAppHost=false

# Chiseled: no shell, a non-root user, listens on 8080. The "-extra" variant
# carries ICU, which Microsoft.Data.SqlClient requires.
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "SoccerManager.API.dll"]
