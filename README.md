# Hiberus Industria Project Templates

[![NuGet Version](https://img.shields.io/nuget/v/Hiberus.Industria.Templates.svg)](https://www.nuget.org/packages/Hiberus.Industria.Templates/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Hiberus.Industria.Templates.svg)](https://www.nuget.org/packages/Hiberus.Industria.Templates/)
[![CI Status](https://github.com/hiberus-industria/hiberus-industria-project-templates/actions/workflows/ci.yml/badge.svg)](https://github.com/hiberus-industria/hiberus-industria-project-templates/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A collection of production-ready .NET project templates for enterprise applications. These templates provide fully configured, opinionated architectures that accelerate development of modern distributed applications using .NET Aspire, Clean Architecture, and industry-standard practices.

## Overview

This template pack is designed for **enterprise .NET developers** who want to:

- **Start new projects quickly** with proven architectural patterns
- **Reduce boilerplate** and initial setup time
- **Follow best practices** for security, observability, and maintainability
- **Build distributed applications** with .NET Aspire orchestration
- **Implement Clean Architecture** from day one

Perfect for teams building microservices, web applications, or cloud-native solutions that require authentication, database access, and modern frontend frameworks.

## Available Templates

### Aspire React Template (`aspire-react`)

A comprehensive full-stack application template with enterprise-grade features:

**Backend (ASP.NET Core)**

- **.NET Aspire** orchestration for distributed applications
- **ASP.NET Core Web API** with OpenAPI documentation
- **Clean Architecture** layers (Domain, Application, Infrastructure)
- **Entity Framework Core** with PostgreSQL
- **CQRS pattern** with MediatR
- **Keycloak integration** for authentication and authorization (OAuth2 + OIDC)
- **OpenTelemetry** for distributed tracing and observability
- **Health checks** and resilience patterns

**Frontend (React)**

- **React 18** with TypeScript
- **Vite** for fast development and optimized builds
- **TanStack Router** for type-safe routing
- **TanStack Query** for server state management
- **OIDC authentication** with Authorization Code + PKCE flow
- **Type-safe API client** auto-generated from OpenAPI

**Infrastructure**

- **PostgreSQL** database with automatic migrations
- **Keycloak** preconfigured with realm import
- **PgAdmin** for database management
- **Docker Compose** configuration via Aspire
- **CI/CD GitHub Actions** workflows

**Generated Project Structure**

```
MyCompany.MyProject/
├── src/
│   ├── MyCompany.MyProject.AppHost/          # Aspire orchestrator
│   ├── Services/
│   │   ├── MyCompany.MyProject.Server/       # ASP.NET Core API
│   │   ├── MyCompany.MyProject.Server.MigrationService/
│   │   └── MyCompany.MyProject.Client/       # React frontend
│   └── Libraries/
│       ├── MyCompany.MyProject.ServiceDefaults/
│       ├── MyCompany.MyProject.Server.Domain/
│       ├── MyCompany.MyProject.Server.Application/
│       └── MyCompany.MyProject.Server.Infrastructure/
├── configuration/keycloak/realms/            # Keycloak realm config
└── MyCompany.MyProject.slnx                  # Solution file
```

## Quick Start

### Prerequisites

- **.NET 10 SDK** or later ([download](https://dotnet.microsoft.com/download))
- **Docker Desktop** for running dependencies ([download](https://www.docker.com/products/docker-desktop))
- **Node.js** (for frontend templates) ([download](https://nodejs.org/))

### 1. Install the Template Pack

From NuGet (once published):

```bash
dotnet new install Hiberus.Industria.Templates
```

From a local build:

```bash
dotnet pack -c Release -o ./artifacts
dotnet new install ./artifacts/Hiberus.Industria.Templates.*.nupkg
```

### 2. Create a New Project

```bash
dotnet new aspire-react -n MyCompany.MyProject
cd MyCompany.MyProject
```

This generates a complete solution with the specified name. All namespaces, folder structures, and configurations are automatically updated.

### 3. Run the Application

```bash
dotnet run --project src/MyCompany.MyProject.AppHost
```

This starts the Aspire orchestrator, which will:

- Launch PostgreSQL and Keycloak containers
- Apply database migrations
- Start the API server
- Start the React development server
- Open the Aspire dashboard in your browser

Access the application:

- **Frontend**: http://localhost:3000
- **API**: http://localhost:5211
- **Aspire Dashboard**: Displayed in terminal after startup
- **Keycloak**: http://localhost:8080
- **PgAdmin**: http://localhost:5050

### 4. Clean Up

```bash
# Stop all containers (Ctrl+C in the terminal running AppHost)

# Uninstall the template pack
dotnet new uninstall Hiberus.Industria.Templates
```

## Template Parameters

### `aspire-react` Template

| Parameter    | Type   | Description                                                     | Default |
| ------------ | ------ | --------------------------------------------------------------- | ------- |
| `-n, --name` | string | **Required**. Name of the project (e.g., `MyCompany.MyProject`) | -       |

**Derived Parameters** (automatically generated):

- `appSlug`: Kebab-case version for identifiers (e.g., `my-company-my-project`)
- `appDisplayName`: Human-readable display name (e.g., `My Company My Project`)

Example with custom name:

```bash
dotnet new aspire-react -n Contoso.InventoryManagement
```

Generates:

- Namespace: `Contoso.InventoryManagement`
- Database: `contoso-inventory-management-database`
- Keycloak realm: `contoso-inventory-management`

## Project Structure Explained

### Clean Architecture Layers

**Domain** (`*.Server.Domain`)

- Core business entities and value objects
- Domain events and interfaces
- No dependencies on other layers

**Application** (`*.Server.Application`)

- Use cases and business logic orchestration
- CQRS command/query handlers (MediatR)
- DTOs and mapping profiles
- Validation rules (FluentValidation)

**Infrastructure** (`*.Server.Infrastructure`)

- Entity Framework Core DbContext
- Database migrations
- Keycloak integration
- External service implementations

**Presentation** (`*.Server`)

- ASP.NET Core Web API
- Controllers and middleware
- OpenAPI configuration
- Authentication/authorization setup

### Service Defaults

Common cross-cutting concerns:

- OpenTelemetry configuration
- Health checks
- Service discovery
- HTTP client resilience policies

## Requirements

- **.NET 10 SDK** or later
- **Docker Desktop** (or Docker Engine) for local development
- **Node.js 18+** and **Yarn** (for frontend templates)
- **Git** for version control

For production deployments:

- Container orchestrator (Kubernetes, Azure Container Apps, etc.)
- PostgreSQL database
- Keycloak server (or alternative OAuth2/OIDC provider)
- Application insights or OpenTelemetry collector

## Development

### Building the Template Pack

```bash
# Build the package
dotnet pack -c Release -o ./artifacts

# Check package contents
unzip -l artifacts/*.nupkg
```

### Testing Templates

```bash
# Install locally
dotnet new install ./artifacts/Hiberus.Industria.Templates.*.nupkg

# Generate test project
dotnet new aspire-react -n TestProject -o ../test-output

# Build and run test project
cd ../test-output/TestProject
dotnet build
dotnet run --project src/TestProject.AppHost
```

### Validation Scripts

```bash
# Validate template configurations
./scripts/validate-template-config.sh

# Test all templates
./scripts/test-templates.sh
```

### Uninstall Development Package

```bash
dotnet new uninstall Hiberus.Industria.Templates
```

## CI/CD

This project includes comprehensive GitHub Actions workflows:

### CI Workflow (`.github/workflows/ci.yml`)

Runs on every push and pull request:

- Validates template configurations
- Builds the NuGet package
- Tests template generation
- Verifies generated projects build successfully
- Runs validation scripts

### Release Workflow (`.github/workflows/release.yml`)

Runs on tags (e.g., `v1.0.0`):

- Uses **GitVersion** for semantic versioning based on conventional commits
- Builds the NuGet package with calculated version
- Creates GitHub Release with auto-generated changelog
- Publishes package to NuGet.org
- Updates documentation

### Manual Release Workflow (`.github/workflows/release-manual.yml`)

For on-demand releases:

- Allows specifying a custom version
- Same build and publish steps as automated release
- Useful for hotfixes or special releases

### Versioning Strategy

- **Semantic Versioning**: MAJOR.MINOR.PATCH
- **GitVersion**: Analyzes commit history using conventional commits
- **Conventional Commits**:
    - `feat:` triggers MINOR bump
    - `fix:` triggers PATCH bump
    - `BREAKING CHANGE:` triggers MAJOR bump

## Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details on:

- Code of Conduct
- Reporting bugs and requesting features
- Development workflow and branch strategy
- Commit message conventions (Conventional Commits)
- Pull request process
- Template development guidelines
- Testing requirements

Quick contribution steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/my-feature`)
3. Make your changes with conventional commits
4. Test thoroughly (build, test templates, validate scripts)
5. Push and create a Pull Request

## Documentation

- **[CONTRIBUTING.md](CONTRIBUTING.md)** - How to contribute to this project
- **[CHANGELOG.md](CHANGELOG.md)** - Version history and changes
- **[LICENSE](LICENSE)** - MIT License
- **Template READMEs** - Each template includes comprehensive usage documentation

## Support

- **Issues**: [GitHub Issues](https://github.com/hiberus-industria/hiberus-industria-project-templates/issues)
- **Discussions**: Use GitHub Discussions for questions and ideas
- **Documentation**: Check template-specific READMEs and this document

## Roadmap

Planned templates:

- **Aspire Angular** - Similar to aspire-react but with Angular
- **Minimal API** - Lightweight API template without Clean Architecture
- **Blazor WebAssembly** - Blazor WASM with Aspire backend
- **Worker Service** - Background processing with RabbitMQ/Azure Service Bus

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Copyright (c) 2026 Hiberus Tecnología S.L.U.

## About Hiberus

[Hiberus](https://www.hiberus.com/) is a leading technology consulting company specializing in digital transformation, cloud solutions, and enterprise software development.

---

**Happy coding!** 🚀
