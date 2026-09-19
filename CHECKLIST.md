# Implementation checklist

Legend: `[ ]` not started, `[~]` implemented but not runtime-verified, `[x]` verified, `[!]` blocked.

## Foundation
- [~] SDK-independent layered contracts
- [~] Module registry and dependency validation
- [~] Operation/error/diagnostic routing contract
- [~] Manifest schema and validation implementation
- [ ] Revit 2027 host integration build gate
- [ ] Main Install 1 internal release gate

## Feature ownership
- [~] Library contracts and manifest validation
- [~] Selection contract boundary
- [~] Representation contract boundary
- [ ] Kitchen
- [ ] Rooms
- [ ] Relationships
- [ ] Export
- [ ] Custom Assets
- [ ] Shared Library
- [~] Settings state contract
- [~] Diagnostics implementation
- [~] Test & Feedback Center contract

## Chair vertical slice
- [~] Stable asset manifest contract
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
