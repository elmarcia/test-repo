# PostgreSQL Init Scripts

These scripts seed deterministic local demo data for the OPS&CREW Continuity PoC.

Execution order:

- `001_create_schemas.sql`: creates modular monolith schema placeholders.
- `002_create_operational_tables.sql`: creates operations tables used by the demo.
- `003_seed_demo_data.sql`: inserts flights, pairings, crew members, standby assignments, and journal entries.
- `004_expand_demo_data.sql`: adds a larger professional product-demo operating day.

The seed data now includes more than 20 flights, at least 8 aircraft rotations, compressed turns, delayed flights, cancelled flights, disrupted pairings, standby crew candidates, airport variety, and overlapping operational scenarios. It is designed to make the Angular Flight Timeline feel like a busy OCC board.

The data is not airline-grade scheduling, legality, or regulatory truth. It is deterministic product-demo data and uses `ON CONFLICT DO NOTHING` so scripts remain safe for local initialization.

To reload these scripts locally, reset the PostgreSQL volume:

```powershell
docker compose down -v
docker compose up --build --force-recreate
```

The `down -v` step is required because PostgreSQL init scripts run only when the database volume is first created.
