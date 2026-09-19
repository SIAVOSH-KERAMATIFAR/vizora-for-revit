# Implementation checklist

Legend: `[ ]` not started, `[~]` in progress, `[x]` verified, `[!]` blocked.

## Foundation
- [~] Layered solution and dependency boundaries
- [~] Module registry and narrow feature contracts
- [~] Operation/error/diagnostic routing contract
- [ ] Revit 2027 host integration build gate
- [ ] Main Install 1 internal release gate

## Feature ownership
- [~] Library contracts and manifest validation
- [~] Selection contract
- [~] Representation contract
- [ ] Kitchen
- [ ] Rooms
- [ ] Relationships
- [ ] Export
- [ ] Custom Assets
- [ ] Shared Library
- [~] Settings
- [~] Diagnostics
- [~] Test & Feedback Center

## Chair vertical slice
- [~] Stable asset manifest
- [~] 2D Plan contract
- [~] Basic 3D contract
- [ ] Full 3D physical family and Revit validation
- [ ] Deterministic approved thumbnail from physical asset
- [ ] Visual QA contact sheet inspected
- [ ] Dimension/material/placement integration gate

## Release discipline
- [x] Exactly three planned main installation packages
- [x] No experimental installers
- [ ] One consolidated feedback package per stage
- [ ] Independent content-pack packaging
