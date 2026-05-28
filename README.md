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
- `GET /api/documents/gendec/{flightId}`
- `GET /api/documents/gendec/{flightId}/html`
- `GET /api/documents/crew-manifest/{flightId}`
- `GET /api/documents/crew-manifest/{flightId}/html`
- `GET /api/reports/recovery-actions`
- `GET /api/reports/recovery-actions/html`

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

## Flight Timeline

The Angular app includes a custom operational Flight Timeline view at:

```text
http://localhost:4200/timeline
```

The timeline uses the existing `GET /api/flights` endpoint and the existing delay/cancel write workflows. Flights are grouped by aircraft registration when available, otherwise by route, and are displayed as horizontally positioned bars using estimated departure/arrival times when present and scheduled times otherwise.

The board is intentionally built with Angular, TypeScript, CSS Grid, and Flexbox. It does not use paid or commercial Gantt/Scheduler libraries and avoids vendor lock-in. Timeline calculations live in a dedicated Angular service rather than in the template.

Timeline V2 adds a denser OCC-style board with aircraft/status filters, an operational-now marker, richer hover detail, rotation continuity connectors, turn-time hints, and subtle live visual cues. From the timeline, select a flight bar to inspect details, delay the flight, cancel the flight, open GENDEC, or open the Crew Manifest. The board refreshes after write actions.

## Application Navigation

The Angular app uses a professional dark operations-control shell with grouped navigation:

- Operations
  - Dashboard
  - Flight Timeline
- Documents
  - Document Center
  - GENDEC
  - Crew Manifest
- Reports
  - Disruption Summary
  - Recovery Actions

Document and report entry points are grouped under landing pages rather than loose action buttons:

```text
http://localhost:4200/documents
http://localhost:4200/documents/gendec
http://localhost:4200/documents/crew-manifest
http://localhost:4200/reports
http://localhost:4200/reports/disruption-summary
http://localhost:4200/reports/recovery-actions
```

## Reporting Architecture

Reporting reads operational state and produces derived outputs. Document/report generation stays under `OpsCrew.Continuity.Core/Modules/Reporting` and does not live in Operations services.

Operational document endpoints:

```text
GET /api/documents/gendec/{flightId}
GET /api/documents/gendec/{flightId}/html
GET /api/documents/crew-manifest/{flightId}
GET /api/documents/crew-manifest/{flightId}/html
```

Operational report endpoints:

```text
GET /api/reports/recovery-actions
GET /api/reports/recovery-actions/html
```

GENDEC and Crew Manifest generation read existing flight, pairing, and crew data and produce derived document previews. Recovery Actions reads disrupted flights, standby assignments, and journal entries to summarize demo recovery decisions. These outputs do not implement regulatory compliance, external authority integrations, or a separate service. HTML previews are returned by the API, and PDF export is supported through browser print-to-PDF in the Angular UI.

Angular preview routes:

```text
http://localhost:4200/documents/gendec
http://localhost:4200/documents/crew-manifest
http://localhost:4200/gendec/{flightId}
http://localhost:4200/crew-manifest/{flightId}
http://localhost:4200/reports/recovery-actions
```

Use the document module pages to select a flight-specific preview. PDF export is handled through the browser's print dialog with the "Print / Export PDF" button inside preview/report contexts.

## Testing New Features Locally

Start the full stack:

```powershell
docker compose up --build
```

Then verify:

- Dashboard remains available at `http://localhost:4200`
- Flight Timeline opens at `http://localhost:4200/timeline`
- Document Center opens at `http://localhost:4200/documents`
- GENDEC selection opens at `http://localhost:4200/documents/gendec`
- Crew Manifest selection opens at `http://localhost:4200/documents/crew-manifest`
- Delay and cancel actions from the timeline refresh the board
- GENDEC preview opens from dashboard or timeline flight actions
- Crew Manifest preview opens from dashboard or timeline flight actions
- Recovery Actions Report opens at `http://localhost:4200/reports/recovery-actions`
- API health remains available at `http://localhost:8080/api/health`
- GENDEC JSON is available at `http://localhost:8080/api/documents/gendec/{flightId}`
- GENDEC HTML is available at `http://localhost:8080/api/documents/gendec/{flightId}/html`
- Crew Manifest JSON is available at `http://localhost:8080/api/documents/crew-manifest/{flightId}`
- Recovery Actions JSON is available at `http://localhost:8080/api/reports/recovery-actions`

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
- `004_expand_demo_data.sql`: expands the product demo with a busier OCC operating day

The demo data includes:

- active, delayed, cancelled, and disrupted flights
- multiple aircraft registrations with chained rotations
- compressed turns and downline delay scenarios
- domestic, international, coastal, island, and regional airport variety
- crew members across assigned, available, standby, and legality review states
- simple pairings with demo legality statuses
- crew legality issue examples
- standby crew assignments
- operational journal entries for monitoring, delay, cancellation, standby, disruption, and crew legality events

The seed scripts are intentionally deterministic and conflict-safe. The expanded demo data now includes more than 20 flights, at least 8 aircraft rotations, additional crew members, pairings, standby assignments, operational journal entries, and recovery action source events. It creates a busy operating day for product demos, not airline-grade scheduling truth.

Reset the local PostgreSQL database and rerun all init scripts:

```powershell
docker compose down -v
docker compose up --build --force-recreate
```

`docker compose down -v` removes the named PostgreSQL volume, so the next `up` starts with a fresh database and reloads every init script. Without `down -v`, PostgreSQL keeps the existing volume and does not rerun scripts from `database/postgres/init`.

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
- custom Angular Flight Timeline V2 with no commercial scheduler/Gantt dependency
- Reporting document generation for GENDEC and Crew Manifest
- Reporting recovery actions report
- expanded demo data for chained rotations and overlapping disruption scenarios

Not implemented yet:

- airline-grade business rules
- real authentication provider integration
- external system integration
- airline-grade scheduling or legality complexity
- real regulatory document filing
- server-side PDF rendering
