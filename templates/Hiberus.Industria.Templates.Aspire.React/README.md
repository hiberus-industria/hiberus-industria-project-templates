# Proyecto generado (Aspire + React)

Este README describe el **proyecto final** que obtienes tras generar una solución con esta plantilla y ejecutar `dotnet new ...`.

La solución resultante está pensada para empezar a desarrollar una aplicación web moderna con:

- **.NET + .NET Aspire** como orquestador de la aplicación distribuida.
- **Backend ASP.NET Core** (API) con **OpenAPI**, autenticación **OIDC/JWT** contra **Keycloak**, y arquitectura por capas.
- **Frontend React** con **Vite**, **TanStack Router**, **TanStack Query** y autenticación OIDC.
- **PostgreSQL** como base de datos, con **EF Core** y un servicio de migraciones.

> Nota: los nombres de los proyectos/espacios de nombres pueden variar según el nombre que indiques al generar (`-n`, `--name`, etc.). En este documento se usan nombres genéricos como `<SolutionName>`.

## Requisitos

- **.NET SDK** según el `global.json` del proyecto (por defecto, `net10.0`).
- **Docker Desktop** (o Docker Engine) para ejecutar dependencias (PostgreSQL, Keycloak, PgAdmin) mediante Aspire.
- **Node.js** y **Yarn** para el frontend.
    - El proyecto está preparado para Yarn moderno (v4+) mediante `packageManager`.
    - Si tu entorno usa Corepack: `corepack enable`.

## Qué se ejecuta cuando levantas la solución

El punto de entrada para desarrollo local es el **AppHost** (Aspire). Desde ahí se levantan, coordinados:

- **PostgreSQL** + volumen persistente.
- **PgAdmin** (para inspección de la BD) expuesto típicamente en `http://localhost:5050`.
- **Keycloak** con importación automática de realms (por defecto `master` y un realm de la app).
- **Server (API)**.
- **MigrationService** (aplica migraciones EF Core y termina).
- **Client (React/Vite)** en `http://localhost:3000`.

## Estructura del repositorio

La solución usa una estructura típica `src/` + librerías compartidas:

```
src/
	<SolutionName>.AppHost/                 # Aspire AppHost: orquesta todo
	Services/
		<SolutionName>.Server/               # API (ASP.NET Core)
		<SolutionName>.Server.MigrationService/ # Worker: ejecuta migraciones EF Core
		<SolutionName>.Client/               # React + Vite + Yarn
	Libraries/
		<SolutionName>.ServiceDefaults/      # Observabilidad, healthchecks, service discovery
		<SolutionName>.Server.Domain/        # Dominio (entidades, reglas)
		<SolutionName>.Server.Application/   # Casos de uso (CQRS/MediatR, validación)
		<SolutionName>.Server.Infrastructure/# Persistencia, repositorios, auth Keycloak
configuration/
	keycloak/
		realms/                              # Import de realms para Keycloak
```

## Componentes en detalle

### 1) AppHost (Aspire)

Proyecto: `src/<SolutionName>.AppHost/`

Responsabilidades principales:

- Define recursos de infraestructura (contenedores) y sus volúmenes.
- Declara referencias entre servicios (por ejemplo, el Server “depende” de DB y Keycloak).
- Arranca el frontend con `AddViteApp(...)` y configura su endpoint.

En la implementación por defecto:

- PostgreSQL se crea como recurso `database` y expone una base `templates-aspire-react-database`.
- PgAdmin se expone en el puerto `5050`.
- Keycloak se expone en el puerto `8080` e importa realms desde `configuration/keycloak/realms/`.
- El Client se expone en el puerto `3000` y espera al Server.

### 2) Server (API)

Proyecto: `src/Services/<SolutionName>.Server/`

Qué incluye:

- API ASP.NET Core con controladores.
- OpenAPI expuesto (por defecto, el cliente se genera desde `http://localhost:5211/openapi/v1.json`).
- Autenticación JWT contra Keycloak.
- Autorización por políticas basada en roles de realm.

Endpoints de soporte (solo en `Development`):

- `GET /health`
- `GET /alive`

### 3) MigrationService

Proyecto: `src/Services/<SolutionName>.Server.MigrationService/`

Es un worker que:

- Arranca, obtiene el `DbContext`.
- Ejecuta `Database.MigrateAsync()` usando estrategia de reintentos.
- Finaliza el proceso al terminar.

Se ejecuta como parte del arranque distribuido, para que la base de datos quede preparada antes de trabajar.

### 4) Frontend (Client)

Proyecto: `src/Services/<SolutionName>.Client/`

Tecnologías:

- React + Vite.
- TanStack Router (rutas por ficheros) y TanStack Query.
- Autenticación OIDC (Authorization Code + PKCE) con `oidc-client-ts` + `react-oidc-context`.

#### Proxy a la API

En desarrollo, Vite configura un proxy para que las llamadas a `/api/...` se enruten al Server. Aspire inyecta las URLs del Server mediante variables de entorno (`services__server__http__0`, `services__server__https__0`).

#### Cliente TypeScript generado desde OpenAPI

El comando `yarn gen:client` usa `@hey-api/openapi-ts` y genera:

- Cliente fetch tipado en `src/client`.
- Helpers para TanStack Query.

Esto reduce “manualidades” en llamadas HTTP y mantiene el contrato API/Front sincronizado.

#### Runtime config (para despliegue)

La app lee configuración desde `window.__RUNTIME_CONFIG__` (inyectada en `runtime-config.js`).

- Plantilla: `public/runtime-config.template.js`
- Lectura centralizada: `src/global-variables.ts`

Si construyes el front como estático con Nginx, el `Dockerfile` incluye un `envsubst` para generar `runtime-config.js` en arranque, sin necesidad de recompilar.

### 5) Libraries (arquitectura por capas)

La solución sigue una separación por responsabilidades:

- **Domain**: entidades y lógica de negocio (sin dependencias de infraestructura).
- **Application**: casos de uso (CQRS con MediatR), validación, contratos.
- **Infrastructure**: EF Core (PostgreSQL), repositorios, integración con Keycloak (authn/authz y cliente admin).
- **ServiceDefaults**: cross-cutting (telemetría OpenTelemetry, health checks, service discovery, resiliencia en HttpClient).

## Autenticación y autorización (Keycloak)

### Realm y clientes

En local, Aspire importa un realm de aplicación (por defecto `templates-aspire-react`) con, entre otros:

- `frontend`: cliente público para el SPA.
- `server`: cliente confidencial asociado al API.
- `swagger`: cliente con service account para escenarios de integración.

### Roles y políticas

El backend define políticas que se resuelven contra roles de realm:

- `administrator`
- `operator`

Y mapea políticas como:

- `HasAdministratorRole`
- `HasOperatorRole`
- `HasOperatorOrAdminRole`

Además, a nivel de aplicación se manejan grupos lógicos:

- `administrators`
- `operators`

### Gestión de usuarios

La API incluye endpoints de gestión de usuarios (por ejemplo, `/users`) que requieren rol `administrator`.

Cuando se crea o resetea un usuario, el backend utiliza el **cliente admin de Keycloak** para operar contra el realm destino.
La contraseña por defecto se controla con `Authentication:KeycloakClient:DefaultUserPassword`.

## Configuración

### Server

Fichero principal: `src/Services/<SolutionName>.Server/appsettings.json`.

Configuración relevante:

- `ConnectionStrings:templates-aspire-react-database`: cadena de conexión a Postgres (en local la inyecta Aspire).
- `Authentication:OAuth:ValidIssuers`: issuer(s) válidos del realm de Keycloak.
- `Authentication:KeycloakClient`: parámetros del cliente admin de Keycloak (URL, token endpoint, realm, client secret, etc.).

> Importante: en `Development` puedes tener valores de ejemplo. En entornos reales, usa secretos/variables de entorno.

### Client

La configuración runtime del front se define vía `runtime-config.js` (en desarrollo normalmente no lo tocas; en despliegue sí):

- `OIDC_AUTHORITY`: autoridad del realm (p.ej. `http://<keycloak>/realms/<realm>`)
- `OIDC_CLIENT_ID`: client id del SPA (p.ej. `frontend`)
- `OIDC_PROFILE_URL`: URL del perfil de usuario en Keycloak
- `DOCS_URL`: URL a documentación

## Cómo arrancar en local (modo recomendado)

Desde la raíz del repositorio generado:

```bash
dotnet run --project src/<SolutionName>.AppHost
```

Qué deberías ver:

- Recursos levantados (Postgres, Keycloak, PgAdmin).
- Server y MigrationService ejecutándose.
- Client accesible en `http://localhost:3000`.

## Desarrollo del frontend sin Aspire (opcional)

Si quieres ejecutar el front “a mano”, necesitas:

1. Un Server accesible.
2. Exportar las variables de entorno que Vite usa para el proxy (`services__server__http__0` o `services__server__https__0`).

Ejemplo (ajusta la URL del server):

```bash
export services__server__http__0=http://localhost:5211
export PORT=3000
cd src/Services/<SolutionName>.Client
yarn install
yarn dev
```

## OpenAPI y regeneración del cliente TS

Con el backend levantado:

```bash
cd src/Services/<SolutionName>.Client
yarn gen:client
```

Esto refresca los tipos y helpers en `src/client` a partir de la especificación OpenAPI del Server.

## Observabilidad

`<SolutionName>.ServiceDefaults` configura:

- OpenTelemetry para logs, métricas y trazas.
- Instrumentación de ASP.NET Core, HttpClient y runtime.
- Exportación OTLP si existe `OTEL_EXPORTER_OTLP_ENDPOINT`.

## Migraciones EF Core (avanzado)

Las migraciones se aplican automáticamente con `MigrationService` al arrancar.
Si necesitas crear una migración nueva (ejemplo orientativo):

```bash
dotnet ef migrations add MyMigration \
	--project src/Libraries/<SolutionName>.Server.Infrastructure \
	--startup-project src/Services/<SolutionName>.Server \
	--context ApplicationDbContext
```

## Troubleshooting

- **Docker no está arrancado**: AppHost no podrá levantar Postgres/Keycloak.
- **401/403 en la API**: revisa que el token tenga roles de realm (`administrator`/`operator`) y que el issuer coincida con `Authentication:OAuth:ValidIssuers`.
- **El front no llega al server**: comprueba el proxy (`/api`) y que exista `services__server__http__0`/`services__server__https__0`.
- **OpenAPI no responde**: verifica que el Server esté levantado y accesible (por defecto `http://localhost:5211/openapi/v1.json`).
