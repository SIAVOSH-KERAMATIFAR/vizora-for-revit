# Architecture decision records

## ADR-0001 — Layered modular architecture
**Status:** Accepted.  **Date:** 2026-09-19

Use Domain, Application, Revit, Infrastructure, UI and Tests with one-way dependencies. This isolates host concerns and permits deterministic domain tests.

## ADR-0002 — Manifest-driven independent content packs
**Status:** Accepted.  **Date:** 2026-09-19

Runtime consumes versioned catalogs. Assets, thumbnails and QA evidence live in independently versioned packs. Invalid content is quarantined and never counted as active.

## ADR-0003 — Preview then Apply
**Status:** Accepted.  **Date:** 2026-09-19

Automated operations produce immutable previews. Application applies only explicit user-approved previews; no Idling-driven mutation is permitted.

## ADR-0004 — Stable logical asset identity
**Status:** Accepted.  **Date:** 2026-09-19

Representation variants may replace physical Revit elements while preserving the logical instance identity and placement state.

## ADR-0005 — Three main installers
**Status:** Accepted.  **Date:** 2026-09-19

The release plan contains exactly three planned main installation packages and one optional final acceptance patch. Experimental installers are not permitted.
