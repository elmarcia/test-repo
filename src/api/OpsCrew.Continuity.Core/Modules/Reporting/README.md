# Reporting Module

Reporting owns derived operational outputs such as documents and reports.

Current reporting capabilities:

- `Documents/Gendec`: GENDEC preview with rules, warnings, field visibility, and HTML rendering.
- `Documents/CrewManifest`: crew manifest preview derived from pairings and crew data.
- `Reports/RecoveryActions`: disruption recovery summary derived from flights, standby assignments, and journal entries.

Reporting reads operational data through Core repository interfaces. It does not own flight mutation logic, crew assignment workflows, or database writes.

PDF export is intentionally handled through browser print-to-PDF from HTML previews. No paid PDF or scheduling products are used.
