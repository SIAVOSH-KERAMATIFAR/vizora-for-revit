# Stage 1 implementation plan

1. Establish SDK-independent projects and dependency checks.
2. Define immutable asset, representation, category, dimension and material contracts.
3. Define operation results, diagnostic context, error ownership and correlation routing.
4. Implement manifest validation and versioned content-pack boundary.
5. Register isolated feature modules and navigation metadata through a composition root.
6. Add the Chair vertical slice as a contract-driven candidate, explicitly quarantined until physical-family and visual QA gates pass.
7. Add MVVM UI shell contracts; prohibit document mutation in UI.
8. Add Revit gateway seams and host registration without hiding unavailable SDK dependencies.
9. Add automated tests for domain rules, manifests, module isolation and preview/apply user-control rules.
10. Produce the internal Stage 1 release evidence package; only then evaluate Main Install 1.

Every batch updates `STATUS.md`, `CURRENT-STATE.json`, `CHECKLIST.md`, and the relevant evidence. User testing remains blocked before Main Install 1 gates pass.
