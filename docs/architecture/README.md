# Architecture Notes

OPS&CREW Continuity PoC starts as a modular monolith:

- one Angular Web Application
- one ASP.NET Core Core API
- one PostgreSQL database

The repository intentionally excludes microservices, Synchronization Control, CDC replication, OPERVUELOS integration, and real Entra ID integration.
