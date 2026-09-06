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
| `SoccerManager.API` | ASP.NET Core Web API | HTTP entry point, controllers, Swagger/OpenAPI, composition root |
| `SoccerManager.Application` | Class library | Commands, queries, handlers, DTOs, validation |
| `SoccerManager.Domain` | Class library | Entities, value objects, domain rules |
| `SoccerManager.Infrastructure` | Class library | Persistence, external integrations |

All four projects target `net10.0` with nullable reference types and implicit
usings enabled. References run inwards: `API` → `Application` + `Infrastructure`,
`Infrastructure` → `Application` + `Domain`, `Application` → `Domain`. The
domain project references nothing.

## CQRS with MediatR

Every use case is a MediatR request with one handler. Controllers never depend
on MediatR directly — they inject `ICommandDispatcher` / `IQueryDispatcher`
from `Application/Core`, which are thin wrappers over `IMediator`.

Each operation gets its own folder holding only that operation's parts:

```
SoccerManager.Application/
  Core/
    Command/     ICommandDispatcher, CommandDispatcher
    Query/       IQueryDispatcher, QueryDispatcher
    User/        ICurrentUser — the caller's id, implemented in the API project
    Behaviors/   ValidationBehavior — runs FluentValidation before the handler
  Commands/<Entity>/<Operation>/   Request, Validator, Mapper, Handler
  Queries/<Entity>/<Operation>/    Request, Response, Mapper, Handler
  Dtos/                            one record per DTO
  Repositories/                    interfaces only; implementations sit in Infrastructure
```

Handlers are registered by assembly scanning, so a new operation needs no DI
change. Mapping is hand-written in a static `Mapper` class per operation — there
is no AutoMapper or Mapster in this solution.

`League` is the reference slice: read it before adding an entity of your own.

### Errors

Handlers throw and `ExceptionHandlingMiddleware` translates:

| Exception | Response |
| --- | --- |
| `ValidationException` (FluentValidation) | 400 with the per-property failures |
| `KeyNotFoundException` | 404 |
| anything else | 500, logged, with no exception detail in the body |

### Persistence

`InMemoryLeagueRepository` is a temporary stand-in that holds rows in process
memory and is registered as a **singleton** — a scoped registration would
discard every row between requests. Data does not survive a restart. Replacing
it with a real implementation is a one-line change in
`SoccerManager.Infrastructure/BuilderExtensions.cs`; nothing in the application
layer changes.

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

The realm ships with no users, deliberately. Create one in the admin console and
give it an email, first name and last name — Keycloak's user profile requires
all three, and a token request for an incomplete account fails with
`Account is not fully set up` rather than anything more obvious.

### Deploying to Azure

[`infra/main.bicep`](infra/main.bicep) creates the container registry, Log
Analytics workspace, Container Apps environment and the Keycloak container app,
and declares the SQL database and its firewall rule.

The image has to exist before the container app can pull it, so the registry
comes first. `az acr build` builds in the cloud, so a local Docker daemon is not
required:

```bash
az acr create -g soccerteambuilder -n soccermanageracr --sku Basic --admin-enabled true
az acr build -r soccermanageracr -t soccermanager-keycloak:latest ./keycloak

set -a; . ./.env; set +a   # credentials, read by main.bicepparam
az deployment group create -g soccerteambuilder --parameters infra/main.bicepparam
```

The parameter file reads every credential from the environment, so nothing
secret is passed on the command line or written to disk. Override the registry
name with `ACR_NAME` if you use a different one.

The deployment's `keycloakAuthority` output is the value for
`Keycloak:Authority` in
[`SoccerManager.API/appsettings.json`](SoccerManager.API/appsettings.json);
`Keycloak:Audience` is `soccermanager-api`.

Keycloak runs on a single replica — more than one needs Infinispan cache
clustering, which is not configured.

## Next steps

- Replace `InMemoryLeagueRepository` with a real SQL Server implementation, and
  add migration scripts for the schema it needs
- Replace the sample `WeatherForecast` endpoint with real team/player endpoints
- Add a test project under the solution's `tests` folder — handler and validator
  tests first, since both are plain classes with no HTTP dependency
