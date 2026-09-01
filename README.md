# SoccerManager

A soccer team management API built on .NET 10, laid out in clean-architecture layers.

> **Status:** early scaffold. The solution structure and build are in place; the
> domain, application and infrastructure layers are still empty, and the API
> exposes only the default `WeatherForecast` sample endpoint.

## Requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or later
- Visual Studio 2022/2026, JetBrains Rider, or VS Code with the C# Dev Kit (optional)

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

## Next steps

- Wire the layers together with project references (API → Application → Domain,
  Infrastructure → Domain)
- Replace the sample `WeatherForecast` endpoint with real team/player endpoints
- Add a persistence provider and a test project under the solution's `tests` folder
