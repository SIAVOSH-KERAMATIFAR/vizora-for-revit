# Dependency rules

Allowed dependencies are strictly one-way:

`Vizora.Domain <- Vizora.Application <- {Vizora.Infrastructure, Vizora.Revit, Vizora.UI}`

`Vizora.Tests` may reference all production contracts, but production projects never reference tests. Domain has no Revit API, WPF, filesystem or network dependency. UI calls application ports only. Features communicate through stable contracts and immutable events, never through another feature's internal implementation or Revit adapter.

A module is registered through the composition root. Adding a module must not require editing unrelated feature internals. Circular references are forbidden and checked by tests/build tooling.
