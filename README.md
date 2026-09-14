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

## Persistence

SQL Server via Dapper, with the schema owned by [Evolve](https://evolve-db.netlify.app/)
migration scripts. There is no EF model and no ORM mapping configuration.

Every operation is a **stored procedure**. This is not a stylistic choice: a SQL
Server scalar function may not perform `INSERT`/`UPDATE`/`DELETE`, so the
function-per-operation pattern used by the sibling PestManagement service does
not transfer.

```
SoccerManager.Infrastructure/Database/
  Connections/   opens SqlConnections from the configured connection string
  Dapper/        the scoped connection + transaction shared across one request
  Migrator/      runs Evolve
  Repositories/  ILeagueRepository implemented against the stored procedures
  Scripts/       the migrations
```

### Migration scripts

| Prefix | Meaning |
| --- | --- |
| `V1_0_0_NN__lower_snake_name.sql` | Versioned. Applied once, in order, then never again. Tables and indexes. |
| `R__PascalCaseName.sql` | Repeatable. Re-applied whenever its checksum changes. One `CREATE OR ALTER PROCEDURE` per file. |

Evolve applies all pending versioned scripts first, then any repeatable script
whose contents changed. It tracks what it has run in a `changelog` table, and
runs with erase disabled so it can never drop the schema.

Two rules for the repeatable scripts: `CREATE OR ALTER PROCEDURE` must be the
first statement in its batch, so each file holds exactly one procedure; and
never write `GO`, which is a client-tool batch separator rather than T-SQL.

Columns are PascalCase and match the entity property names exactly, so Dapper
maps them with no configuration.

Timestamps are `DATETIME2(7)` holding UTC — not `DATETIMEOFFSET`, since every
value written is `DateTime.UtcNow` and the offset would be a constant. Because
`DATETIME2` carries no zone, `UtcDateTimeHandler` marks every value Dapper reads
back as UTC; without it the same field would serialize with a trailing `Z` when
freshly created and without one when read from the database.

### Configuration

The connection string is read from `ConnectionStrings:soccermanager`. It is
**empty in `appsettings.json` on purpose** — supply the real value through the
git-ignored `.env`, where the double underscore is .NET's own configuration
separator:

```bash
set -a; . ./.env; set +a
dotnet run --project SoccerManager.API
```

The API fails at startup, rather than at the first request, when it is missing.

`Database:RunMigrationsOnStartup` controls whether Evolve runs during startup. It
is **`true` in Development and `false` otherwise**, so a deployment applies
schema changes as its own step instead of implicitly on every replica start.

`GET /health` reports the database connection.

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
| `soccermanager-spa` | public | The React web app signs in through it: Authorization Code with PKCE (`S256`), no direct grants, no implicit flow. Its audience mapper puts `soccermanager-api` into every access token. |

The SPA client's redirect URIs and web origins list the Vite dev server,
`http://localhost:5173`; a deployed frontend origin is added alongside it once
one exists. The same origin has to be in `Cors:AllowedOrigins` in
[`SoccerManager.API/appsettings.json`](SoccerManager.API/appsettings.json), or
the browser blocks the call before the API ever sees the token.

There is no password grant. To call the API by hand, sign in through the web
app and copy the bearer token from one of its requests.

### The realm file seeds an empty database only

`--import-realm` skips a realm that already exists, so editing the JSON and
rebuilding the image changes nothing on a Keycloak whose database already holds
the realm. Local and Azure share that database, so one admin-console change —
made through either — is what actually changes the running realm; make it
there, and keep the JSON in step by hand, since it is what a fresh environment
starts from.

### The login theme

The login page is a custom theme at
[`keycloak/themes/soccermanager/`](keycloak/themes/soccermanager/), extending
Keycloak's `base` theme, and it is baked into the image the same way the
realm is.

It overrides only `theme.properties` and `template.ftl`. Every other page —
reset password, update password, OTP, error — inherits base's template and
picks up our styling through the class hooks declared in `theme.properties`.
Those two files are copies of Keycloak's own, so on a Keycloak major upgrade
they should be diffed against the new `base/login` equivalents.

Locally, the theme folder is bind-mounted and theme caching is off, so an
edit under `keycloak/themes` is visible on the next page load without a
rebuild.

Applying the theme to a realm that already exists needs an explicit change,
because `--import-realm` skips a realm that already exists: rebuilding the
image changes nothing for it. Set it through the admin console, under Realm
settings → Themes → Login theme, or with

```bash
kcadm.sh update realms/soccermanager -s loginTheme=soccermanager
```

`rememberMe` was turned on in the realm file the same way and needs the same
treatment on a live realm.

**Ordering matters here**: the image carrying the theme has to be built and
deployed before a live realm is pointed at it, or the login page breaks for
everyone. Local and Azure Keycloak share one database, so the switch is
global and immediate.

### Running Keycloak locally

Keycloak's store is the `keycloak` database on the shared Azure SQL server, so
there is no local SQL container. Two consequences: your machine's public IP
needs a firewall rule on that server, and realm changes you make locally are
visible to everyone.

```bash
cp .env.example .env   # then fill in the SQL and admin credentials
docker compose up --build
```

The admin console is at <http://localhost:8080>.

The realm ships with no users, deliberately. Create one in the admin console and
give it an email, first name and last name — Keycloak's user profile requires
all three, and a sign-in for an incomplete account fails with
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

- Replace the sample `WeatherForecast` endpoint with real team/player endpoints
- Add a test project under the solution's `tests` folder — handler and validator
  tests first, since both are plain classes with no HTTP dependency
- Move Keycloak off the SQL Server admin login onto a contained user, so it holds
  no rights over the application database
- The application database is Basic tier (5 DTU); revisit before it carries load
