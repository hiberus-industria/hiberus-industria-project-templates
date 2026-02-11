# Hiberus Industria Project Templates

A collection of .NET Aspire project templates for enterprise applications with Clean Architecture, Entity Framework Core, and Keycloak authentication.

## Templates

### Aspire React Template (`aspire-react`)

Full-stack application template with:

- **.NET Aspire** orchestration
- **React** frontend with TypeScript and Vite
- **ASP.NET Core** Web API backend
- **Clean Architecture** (Domain, Application, Infrastructure layers)
- **Entity Framework Core** with PostgreSQL
- **Keycloak** authentication and authorization
- **OpenTelemetry** observability

## Installation

Install the template pack from NuGet:

```bash
dotnet new install Hiberus.Industria.Templates
```

Or install from a local package:

```bash
dotnet new install ./artifacts/Hiberus.Industria.Templates.0.1.0.nupkg
```

## Usage

Create a new project from the template:

```bash
dotnet new aspire-react -n MyCompany.MyProject
```

This will create a new solution with the complete project structure.

### Build and Run

```bash
cd MyCompany.MyProject
dotnet restore
dotnet build
dotnet run --project src/MyCompany.MyProject.AppHost
```

## Uninstall

```bash
dotnet new uninstall Hiberus.Industria.Templates
```

## Development

To build the template pack locally:

```bash
dotnet pack -c Release -o ./artifacts
```

## License

MIT
