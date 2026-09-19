# Automated test structure

- `Domain/`: pure rules and invariants.
- `Application/`: preview/apply and cancellation workflows.
- `Contracts/`: manifest, schema, error and module registry contracts.
- `Features/<Feature>/`: feature-owned integration fakes and boundary tests.
- `ReleaseGates/`: machine-checkable evidence checks; failures block installer eligibility.

Tests must run without Revit where possible. Revit-dependent tests are explicit host tests and cannot be silently treated as passed by fakes.
