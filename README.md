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
|-- Crew/
|-- Flights/
|-- Health/
|-- Journal/
`-- Pairings/
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

Read-only operational API endpoints:

- `GET /api/flights`
- `GET /api/flights/disrupted`
- `GET /api/crew-members`
- `GET /api/standby-assignments`
- `GET /api/pairings`
- `GET /api/journal`

Demo write workflow endpoints:

- `POST /api/flights/{flightId}/delay`
  - body: `{ "minutes": 30, "reason": "Operational continuity demo delay" }`
  - marks the flight as delayed, updates estimated times, and adds a journal entry
- `POST /api/flights/{flightId}/cancel`
  - body: `{ "reason": "Operational continuity demo cancellation" }`
  - marks the flight as cancelled and adds a journal entry
- `POST /api/standby-assignments/{standbyAssignmentId}/assign`
  - body: `{ "flightId": "FLT-1002", "notes": "Assigned from continuity dashboard" }`
  - marks the standby assignment as assigned and adds a journal entry
- `POST /api/journal`
  - body: `{ "severity": "INFO", "category": "Demo", "flightId": null, "crewMemberId": null, "message": "Manual demo journal entry" }`
  - adds a manual operational journal entry

These write endpoints are intentionally simple PoC workflows. They do not implement airline-grade scheduling, crew legality rules, authentication, CDC, Synchronization Control, OPERVUELOS integration, or event-driven processing.

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

The official PostgreSQL container loads these scripts automatically from `/docker-entrypoint-initdb.d` the first time a new database volume is created. Docker Compose mounts the local folder into that container path:

```yaml
./database/postgres/init:/docker-entrypoint-initdb.d:ro
```

Scripts are executed in filename order:

- `001_create_schemas.sql`: creates schema placeholders for the modular monolith
- `002_create_operational_tables.sql`: creates simple demo operational tables in the `operations` schema
- `003_seed_demo_data.sql`: inserts deterministic fake continuity data

The demo data includes:

- active, delayed, cancelled, and disrupted flights
- crew members across assigned, available, standby, and legality review states
- simple pairings with demo legality statuses
- crew legality issue examples
- standby crew assignments
- operational journal entries for monitoring, delay, cancellation, standby, disruption, and crew legality events

Reset the local PostgreSQL database and rerun all init scripts:

```powershell
docker compose down -v
docker compose up --build
```

`docker compose down -v` removes the named PostgreSQL volume, so the next `up` starts with a fresh database and reloads every init script.

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
- read-only Core API endpoints for demo operational data
- simple write workflows for delay, cancellation, standby assignment, and journal entry creation

Not implemented yet:

- airline-grade business rules
- real authentication provider integration
- external system integration
- airline-grade scheduling or legality complexity
