# Modules

Feature modules for the web application belong here.

Keep this aligned with the Core API module boundaries.

Current modules:

- `dashboard`: operational summary and entry point into flight actions and reporting previews.
- `flight-timeline`: custom OCC-style timeline built with Angular, TypeScript, CSS Grid, Flexbox, and native browser rendering. Timeline calculations live in `flight-timeline.service.ts`.
- `gendec-preview`: Reporting document preview for GENDEC.
- `crew-manifest-preview`: Reporting document preview for Crew Manifest.
- `recovery-actions-report`: Reporting preview for disruption recovery actions.

The timeline deliberately avoids commercial Gantt/Scheduler products and keeps the rendering explainable for a PoC that can evolve into an enterprise platform.
