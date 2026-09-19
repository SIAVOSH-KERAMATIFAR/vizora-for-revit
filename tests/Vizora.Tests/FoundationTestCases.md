# Foundation automated test inventory

The foundation batch defines the following executable test cases for the SDK-independent test project:

- Valid manifests are accepted.
- Missing stable IDs, thumbnails, Plan 2D or Basic 3D representations are rejected.
- Duplicate dimensions, material slots and catalog asset IDs are rejected.
- Missing module dependencies and dependency cycles are rejected.
- Diagnostic routing returns only errors for the requested correlation ID.
- Operation results distinguish success from routed failure.
- Default module registration exposes only verified foundation states.

The current API-only repository session has no .NET SDK execution environment, so these tests are recorded as source-ready cases and are not claimed as executed.
