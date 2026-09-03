# SoccerManager

A soccer team management API built on .NET 10, laid out in clean-architecture layers.

> **Status:** early scaffold. The solution structure and build are in place; the
> domain, application and infrastructure layers are still empty, and the API
> exposes only the default `WeatherForecast` sample endpoint.

## Requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or later
- Visual Studio 2022/2026, JetBrains Rider, or VS Code with the C# Dev Kit (optional)
- [Docker](https://www.docker.com/products/docker-desktop/), to run Keycloak locally
- [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli), to deploy it

## Solution layout

| Project | Type | Purpose |
| --- | --- | --- |
| `SoccerManager.API` | ASP.NET Core Minimal API | HTTP entry point, Swagger/OpenAPI, composition root |
| `SoccerManager.Application` | Class library | Use cases, application services, DTOs |
| `SoccerManager.Domain` | Class library | Entities, value objects, domain rules |
| `SoccerManager.Infrastructure` | Class library | Persistence, external integrations |

All four projects target `net10.0` with nullable reference types and implicit
usings enabled.

## Getting started

```bash
git clone https://github.com/panche-trifunov-softela/Softela.SoccerManager.git
cd Softela.SoccerManager
dotnet restore
dotnet build
dotnet run --project SoccerManager.API
```

The API launches with Swagger UI enabled in the Development environment:

- HTTPS — <https://localhost:7265/swagger>
- HTTP — <http://localhost:5005/swagger>

Ports are defined in
[`SoccerManager.API/Properties/launchSettings.json`](SoccerManager.API/Properties/launchSettings.json).

## Keycloak

The API authenticates with Keycloak-issued JWT bearer tokens. The realm is
defined in
[`keycloak/realm/soccermanager-realm.json`](keycloak/realm/soccermanager-realm.json)
and baked into the image, so the same realm applies locally and in Azure.

Two clients, split by job:

| Client | Type | Purpose |
| --- | --- | --- |
| `soccermanager-api` | bearer-only | The audience the API validates against. No secret, no enabled flow. |
| `soccermanager-dev` | public | Issues test tokens while no frontend exists. Direct access grants only; retire it once a real frontend client is added. |

### Running Keycloak locally

Keycloak's store is the `keycloak` database on the shared Azure SQL server, so
there is no local SQL container. Two consequences: your machine's public IP
needs a firewall rule on that server, and realm changes you make locally are
visible to everyone.

```bash
cp .env.example .env   # then fill in the SQL and admin credentials
docker compose up --build
```

The admin console is at <http://localhost:8080>. To fetch a token:

```bash
curl -d client_id=soccermanager-dev -d grant_type=password \
     -d username=<user> -d password=<pass> \
     http://localhost:8080/realms/soccermanager/protocol/openid-connect/token
```

### Deploying to Azure

[`infra/main.bicep`](infra/main.bicep) creates the container registry, Log
Analytics workspace, Container Apps environment and the Keycloak container app,
and declares the SQL database and its firewall rule.

The image has to exist before the container app can pull it, so the registry
comes first. `az acr build` builds in the cloud, so a local Docker daemon is not
required:

```bash
az acr create -g soccerteambuilder -n <registry> --sku Basic --admin-enabled true
az acr build -r <registry> -t soccermanager-keycloak:latest ./keycloak
az deployment group create -g soccerteambuilder \
  -f infra/main.bicep -p infra/main.bicepparam -p registryName=<registry>
```

The deployment's `keycloakAuthority` output is the value for
`Keycloak:Authority` in
[`SoccerManager.API/appsettings.json`](SoccerManager.API/appsettings.json);
`Keycloak:Audience` is `soccermanager-api`.

Keycloak runs on a single replica — more than one needs Infinispan cache
clustering, which is not configured.

## Next steps

- Wire the layers together with project references (API → Application → Domain,
  Infrastructure → Domain)
- Replace the sample `WeatherForecast` endpoint with real team/player endpoints
- Add a persistence provider and a test project under the solution's `tests` folder
