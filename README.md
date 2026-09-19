# VIZORA for Revit

Clean implementation for Autodesk Revit 2027. `PROJECT-SPEC.md` is the authoritative contract.

## Status

Stage 1 foundation and Chair vertical-slice contracts are being implemented. No asset or installer is advertised as release-ready until its quality and release gates pass.

## Boundaries

- `Vizora.Domain`: pure models and rules.
- `Vizora.Application`: use cases and ports.
- `Vizora.Revit`: Revit adapters and transactions.
- `Vizora.Infrastructure`: storage, manifests, logging and support packages.
- `Vizora.UI`: presentation and navigation only.
- `Vizora.Tests`: unit, contract and release-gate tests.

See `docs/architecture/DEPENDENCY-RULES.md` and `docs/stages/STAGE-1-PLAN.md`.
