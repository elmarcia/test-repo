# OPS&CREW Continuity PoC

Initial repository scaffold for the OPS&CREW Continuity proof of concept.

## Architecture

This repository is organized as a cloud-provider neutral modular monolith:

- Web Application: Angular
- Core API: ASP.NET Core Web API
- Database: PostgreSQL
- Local orchestration: Docker Compose

The PoC deliberately has one Core API only. It does not include microservices, Synchronization Control, CDC replication, OPERVUELOS integration, or real Entra ID integration.

## Repository Structure

```text
.
|-- database/
|   `-- postgres/
|       `-- init/
|-- deploy/
|   `-- docker/
|-- docs/
|   `-- architecture/
|-- src/
|   |-- api/
|   |   |-- OpsCrew.Continuity.Api/
|   |   |-- OpsCrew.Continuity.Contracts/
|   |   |-- OpsCrew.Continuity.Core/
|   |   |-- OpsCrew.Continuity.Infrastructure/
|   |   `-- OpsCrew.Continuity.slnx
|   `-- web/
|       `-- ops-crew-continuity-web/
|-- tests/
|   `-- api/
|-- docker-compose.yml
`-- README.md
```

## Backend Layout

```text
src/api/OpsCrew.Continuity.Api
|-- CompositionRoot/
|-- Controllers/
`-- Dockerfile

src/api/OpsCrew.Continuity.Core
|-- Abstractions/
|-- Common/
`-- Modules/
    |-- Continuity/
    |-- Crew/
    |-- Identity/
    `-- Operations/

src/api/OpsCrew.Continuity.Infrastructure
|-- Persistence/
`-- Modules/
    |-- Continuity/
    |-- Crew/
    |-- Identity/
    `-- Operations/

src/api/OpsCrew.Continuity.Contracts
`-- Health/
```

## Frontend Layout

```text
src/web/ops-crew-continuity-web
|-- src/
|   |-- app/
|   |   |-- core/
|   |   |-- layout/
|   |   `-- modules/
|   |-- assets/
|   `-- environments/
|-- angular.json
|-- Dockerfile
|-- nginx.conf
`-- package.json
```

## Local Development

Run the full stack with Docker:

```powershell
docker compose up --build
```

Endpoints:

- Web Application: http://localhost:4200
- Core API health: http://localhost:8080/api/health
- PostgreSQL: localhost:5432

Run the Core API locally:

```powershell
dotnet run --project src/api/OpsCrew.Continuity.Api/OpsCrew.Continuity.Api.csproj
```

Run the Angular app locally after installing dependencies:

```powershell
cd src/web/ops-crew-continuity-web
npm install
npm start
```

On Windows PowerShell, use `npm.cmd` if script execution policy blocks the `npm.ps1` shim.

## Database

PostgreSQL initialization scripts live in:

```text
database/postgres/init
```

The current script creates schema placeholders only. No tables or business data have been added.

## Container Portability

The Dockerfiles and Compose file avoid cloud-provider specific services. The containers can be built and run locally, then adapted for Azure, AWS, Kubernetes, or another OCI-compatible runtime.

## Current Scope

Implemented:

- repository structure
- Angular skeleton
- ASP.NET Core solution skeleton
- modular monolith folder organization
- PostgreSQL initialization folder
- Docker Compose
- Dockerfiles
- README

Not implemented yet:

- business logic
- user screens
- real authentication provider integration
- external system integration
- database persistence implementation
