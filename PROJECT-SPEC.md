# VIZORA for Revit — Master Build Brief for an AI Developer

> **Status:** APPROVED BY PRODUCT OWNER  
> **Approved date:** 2026-09-19  
> **Document version:** 1.0 Final  
> **Repository role:** Authoritative product, architecture, workflow and acceptance contract  
> **Target GitHub account:** `SIAVOSH-KERAMATIFAR`  
> **Target repository:** `SIAVOSH-KERAMATIFAR/vizora-for-revit`  
> **Repository URL:** `https://github.com/SIAVOSH-KERAMATIFAR/vizora-for-revit`  
> **Required repository filename:** `PROJECT-SPEC.md`

## Direct bootstrap command for the AI developer

Read this entire document before changing or generating code. Work only in `SIAVOSH-KERAMATIFAR/vizora-for-revit` and create a new clean VIZORA for Revit implementation from this specification. Do not import undocumented source from an older implementation. Do not ask the user to repeat requirements already recorded here.

### Account and environment isolation

This project will be started from a **different GitHub/AI developer account** than the account or chat in which this specification was prepared.

Assume that you have no access to:

- Any previous ChatGPT account or conversation.
- Any previous GitHub account or repository.
- Any old VIZORA source code, branches, commits or releases.
- Any earlier ZIP, Support Package or local workspace.
- Any connected Library, connector, memory or hidden project context.

Do not search for, request or depend on those unavailable resources. Everything required to define the new project is contained in this document. The new repository created in the other account becomes the source of truth from its first commit.

If the user uploads this file directly to an AI coding chat before creating a repository, first use it to propose the new repository structure and then create/populate the new repository available in that account. If the file is already stored in the repository as `PROJECT-SPEC.md`, read it from there and proceed.

First commit the governance documents, architecture boundaries, feature map, machine-readable checklist/state, tests and Stage 1 implementation plan required by this specification. Then begin implementation and continue autonomously through the Stage 1 delivery gate. Do not send the user an installer until the complete Main Install 1 gate passes.

This approved specification must not be silently rewritten by an implementation agent. Proposed scope or architecture changes must be documented and presented to the user for approval before they replace this contract.

## Mandatory instruction

You are the lead software architect and implementation agent for a **new, clean implementation** of VIZORA for Revit.

This document is self-contained. Do not assume access to an older repository, an earlier chat, previous ZIP files, or undocumented decisions. Do not ask the user to restate the project history. Treat this document as the product contract, architecture contract, development workflow, quality standard, progress checklist, and acceptance policy.

Do not begin by implementing every feature at once. First create the modular foundation, project documentation, test infrastructure, release pipeline, and one complete vertical slice. Then implement the remaining capabilities module by module.

The primary target is:

- Product: **VIZORA for Revit**
- Host: **Autodesk Revit 2027**
- Platform: **Windows x64**
- UI: **one VIZORA Ribbon tab and one main Dockable Panel**
- First supported host: Revit 2027 only
- Revit 2025 and 2026 are future backports after the 2027 version is stable

---

# Part I — Project identity and product vision

## 1. What VIZORA is

VIZORA is not a basic Revit family library. It is a lightweight, extensible and user-controlled interior design system for Revit.

It must eventually provide:

- A visual furniture and interior asset library
- Kitchen and cabinet components
- Appliances
- Custom assets
- Shared/team libraries
- Independent 2D and 3D representations
- Component-level dimensions and materials
- Smart kitchen layout
- Smart room presets
- Relationships between objects
- Export preparation for Twinmotion, Unreal Engine and Datasmith
- Diagnostics, auditing and support packages

The user must work in normal Revit Plan, Section, Elevation and 3D views. VIZORA must not create a separate design canvas.

## 2. Primary product goals

1. Keep Revit projects lightweight.
2. Give the user direct control over how VIZORA assets are represented.
3. Provide furniture that looks credible, not placeholder boxes.
4. Preserve Revit-native editing wherever practical.
5. Make common interior layouts faster without removing manual control.
6. Make the library easy to browse and expand.
7. Keep every software feature isolated enough to debug, replace or extend independently.
8. Minimize the amount of installation and debugging the user must perform.

## 3. Non-negotiable user-control rules

- Smart behavior must default to `Ask Before Update`.
- Major automated operations must use `Preview → Apply`.
- Manual Override must always remain available.
- Smart operations must not immediately undo manual movement or rotation.
- Custom assets do not require a defined Front direction.
- Smart Rotation for custom assets is OFF by default.
- No automatic follower movement on Revit `Idling`.
- No heavy family generation or warm-up on Revit `Idling`.
- Representation is chosen by the user and is not forced by view type.

---

# Part II — Fundamental architecture rule: feature isolation

## 4. The most important architecture requirement

The application must be organized as separate, replaceable feature modules with explicit contracts.

If a Kitchen operation fails, investigation must start inside the Kitchen module and its declared dependencies. It must not require searching through unrelated Room, Export, Library or UI code.

If the user later changes:

- a category name,
- a category hierarchy,
- a tab,
- the UI style,
- the asset-card design,
- a cabinet rule,
- an export profile,
- or adds an entirely new capability,

the change must be possible without rewriting the whole plugin.

Features may cooperate only through stable interfaces, shared contracts and application events. They must not reach into each other's internal implementation.

## 5. Required solution layers

Create the solution with these dependency boundaries:

### `Vizora.Domain`

Pure business models and rules. No Revit API, WPF, filesystem or network dependencies.

Owns:

- Asset identity and metadata
- Representation models
- Dimension and material contracts
- Relationship rules and geometry math
- Kitchen planning rules
- Room-preset rules
- Project-state models
- Validation rules
- Schema migration rules

### `Vizora.Application`

Use cases and orchestration. It depends on Domain but not on concrete Revit or filesystem implementations.

Owns:

- Commands, queries and results
- Feature service interfaces
- Transaction-neutral workflows
- Preview models
- Validation coordination
- Error/result contracts
- Cancellation and progress contracts

### `Vizora.Revit`

All concrete Revit API integration.

Owns:

- `IExternalApplication`
- Ribbon and Dockable Panel registration
- External Commands and External Events
- Revit transaction gateway
- Selection adapters
- Element placement and replacement
- Parameter and material mapping
- View and category adapters
- Extensible Storage integration
- Revit-specific failure processing

### `Vizora.Infrastructure`

External storage and operating-system concerns.

Owns:

- JSON manifests
- User settings
- Local cache
- Shared-library folders
- Thumbnail cache
- Logging
- Audit reports
- Support ZIP generation
- File-lock-safe temporary files

### `Vizora.UI`

Presentation only.

Owns:

- Dockable Panel views
- View models
- Theme and design tokens
- Navigation
- Validation presentation
- Preview presentation

The UI must not open Revit transactions or mutate the Revit document directly. All Revit operations must pass through the Application contracts and the Revit gateway.

### `Vizora.Tests`

Owns:

- Domain unit tests
- Application workflow tests
- Contract tests
- Manifest/schema tests
- Migration tests
- Feature integration tests using fakes where Revit is unavailable
- Release-gate checks

## 6. Required feature modules

Under the appropriate layers, create explicit feature areas:

- `Features/Library`
- `Features/Selection`
- `Features/Representation`
- `Features/Kitchen`
- `Features/Rooms`
- `Features/Relationships`
- `Features/Export`
- `Features/CustomAssets`
- `Features/SharedLibrary`
- `Features/Settings`
- `Features/Diagnostics`
- `Features/TestFeedback`

Each feature must own:

- Its domain objects, when feature-specific
- Application commands/queries
- Interfaces/ports
- Revit adapters
- UI view/view-model or workspace section
- Tests
- Documentation
- Error-code range
- Checklist section

Each feature must expose a narrow public API. Internal classes should not be referenced by other feature modules.

## 7. Dependency rules between features

- A feature cannot directly call another feature's Revit implementation.
- A feature cannot directly modify another feature's UI state.
- Shared behavior must be expressed through Domain contracts, Application interfaces or events.
- Cross-feature events must contain immutable data and stable IDs, not concrete UI or Revit objects.
- Feature registration must use a module registry/composition root.
- Circular dependencies are forbidden.
- A new feature must be addable through registration, navigation metadata and contracts without editing unrelated feature internals.

## 8. Error ownership and diagnostic routing

Every operation must include:

- `OperationId`
- `FeatureId`
- `ErrorCode`
- `CorrelationId`
- Human-readable message
- Technical diagnostic message
- Exception details in logs only
- Document and Revit version context
- Relevant Asset IDs and Element IDs

Use feature-owned error prefixes:

- `LIB-*` — Library
- `SEL-*` — Selection
- `REP-*` — Representation
- `KIT-*` — Kitchen
- `ROM-*` — Rooms
- `REL-*` — Relationships
- `EXP-*` — Export
- `CUS-*` — Custom Assets
- `SHR-*` — Shared Library
- `SET-*` — Settings
- `DIA-*` — Diagnostics
- `TST-*` — Test & Feedback Center
- `INS-*` — Build/Installer

When an error is reported, use `FeatureId`, `ErrorCode` and `CorrelationId` to inspect the responsible feature first. Do not make broad unrelated rewrites.

---

# Part III — Extensibility and configuration

## 9. Category system

Asset categories must not be hard-coded throughout the UI.

Create a category registry driven by versioned data/configuration. A category definition should include:

- Stable Category ID
- Display name
- Parent Category ID, if nested
- Icon key
- Sort order
- Revit category mapping
- Supported dimension keys
- Supported material slots
- Supported representation types
- Optional feature capabilities

Renaming a display label must not change the stable ID or break saved projects.

Adding a category should require:

1. Adding or updating a category definition.
2. Adding relevant assets and validation.
3. Registering optional category-specific behavior.

It must not require rewriting the Library panel.

Initial category scope:

- Chairs
- Dining Chairs
- Armchairs
- Sofas
- Beds
- Desks
- Dining Tables
- Coffee Tables
- TV Units
- Wardrobes
- Shelves
- Kitchen Base Cabinets
- Kitchen Wall Cabinets
- Kitchen Tall Cabinets
- Kitchen Islands
- Refrigerators
- Ovens
- Cooktops
- Sinks
- Dishwashers
- Basic Lighting

## 10. UI extensibility

Do not build one giant Dockable Panel code-behind.

Use:

- MVVM or an equivalent strict presentation separation
- One navigation shell
- Independently registered workspaces
- Shared design tokens
- Reusable controls for cards, filters, fields, status and errors
- Central theme resources
- Feature-specific view models

Changing colors, spacing, typography or asset-card appearance should be possible through the theme/design-system layer.

Adding or removing a workspace should be performed in the module/navigation registry, not through large switch statements distributed across the codebase.

Required main workspaces:

- `LIBRARY`
- `SELECTED`
- `SCENE`
- `KITCHEN`
- `ROOMS`
- `EXPORT`
- `SETTINGS`

Diagnostics may live under Settings or a developer/support section rather than becoming a permanent main tab.

## 11. Asset and preset extensibility

- Asset metadata must be manifest-driven.
- Export profiles must be data-driven where possible.
- Room presets must use versioned definitions plus tested algorithms.
- Kitchen modules and standard sizes must be configurable through validated rules.
- UI labels are not identifiers.
- Stored data must use stable IDs and schema versions.

---

# Part IV — User experience and visual structure

## 12. Revit workflow

The user works directly in the active Revit project:

1. Open a normal Plan, Section, Elevation or 3D view.
2. Open the VIZORA Dockable Panel from the VIZORA Ribbon tab.
3. Browse or search the Library.
4. Place an asset in the active Revit view.
5. Select it and edit dimensions/materials from Revit Properties or VIZORA Selected.
6. Use grips where supported.
7. Change representation for an object, a view scope or the project.
8. Use Kitchen or Room workflows through Preview and Apply.

Do not open a separate editor merely to furnish a room.

## 13. Ribbon requirements

Use one Ribbon tab named `VIZORA`.

Keep the Ribbon compact. It may contain:

- Open/Focus Panel
- Quick representation presets
- Audit/Diagnostics
- Settings/About

Do not create several floating permanent windows.

## 14. Dockable Panel requirements

### Library workspace

- Visual asset cards with real thumbnails
- Search by name, ID, category, style and tags
- Category filter
- Style filter
- Source filter: All / Core / Custom / Shared
- Favorites
- Recent
- Clear asset count
- Representation availability on the card
- Relevant size summary

### Selected workspace

When one supported VIZORA asset is selected, show:

- Name and Asset ID
- Current representation
- Width, Depth, Height and relevant dimensions
- Component material slots
- Relationship status
- Manual Override
- Optional Front/Smart Rotation state

If nothing compatible is selected, show a useful explanation. Never leave the workspace blank.

### Scene workspace

- Representation scope: Selected / Current View / Whole Project
- Optional category scope
- Preview count before applying
- Current representation summary
- Presets: Documentation, Exterior Render, Interior Working, Interior High

### Kitchen workspace

- Select walls/edges
- Layout type
- Required appliances/components
- Standard sizing rules
- Preview
- Conflict list
- Apply
- Auto Reflow state
- Manual Override state

### Rooms workspace

- Living presets
- Dining presets
- Bedroom presets
- Anchor and relationship preview
- Apply/Detach relationships

### Export workspace

- Exterior Render
- Twinmotion Working
- Unreal Final
- Custom export-preparation profile
- Preview of representation/category changes
- No destructive export preparation without user confirmation

### Settings workspace

- Library paths
- Shared/Team Library folders
- Cache location and safe cleanup
- Default smart-update mode
- Default representation
- Audit/Support report destination chosen by the user
- Version and environment information

---

# Part V — Asset system and quality contract

## 15. Asset identity

Every asset must have a stable identity independent of its family filename.

Example IDs:

- `VZ_CH_001`
- `VZ_SF_001`
- `VZ_KB_001`

Every asset manifest must include:

- Schema version
- Asset version
- Stable Asset ID
- Display name
- Category ID
- Optional subcategory
- Style collections
- Tags
- Revit category
- Thumbnail
- Representation variants
- Dimension definitions
- Material-slot definitions
- Optional parameter limits
- Optional forward axis
- Optional relationship anchors/connectors
- Source type: Core / Custom / Shared

## 16. Furniture quality

Furniture must never be represented as an unexplained primitive box.

The lightweight version may be simple, but it must visually read as the correct object.

### Chair

Basic 3D must include:

- Seat
- Back
- Legs/frame
- Correct proportions

Full 3D may add:

- Refined profiles
- Cushion separation
- Better edges and curves
- Upholstery seams or piping when appropriate
- Smaller visible components without excessive polygon cost

### Sofa

Basic 3D must include:

- Body
- Arms
- Seat cushions
- Back cushions
- Legs or base

Full 3D may add refined cushions, piping, frame details and softer profiles.

### Cabinet

Must support meaningful parts such as:

- Carcass
- Door/drawer fronts
- Plinth
- Countertop where relevant
- Handles
- Interior where relevant
- Appliance finish where relevant

## 17. Representation system

VIZORA controls representation independently from Revit's Coarse/Medium/Fine levels.

Required modes:

- `2D Plan`
- `Basic 3D`
- `Full 3D`

The user may intentionally choose 2D Plan while in a 3D workflow or choose 3D for assets used in plan. Do not automatically force a mode based on view type.

### 2D Plan quality

2D graphics must resemble useful architectural CAD blocks, not generic rectangles.

Examples:

- Chair: seat/back/leg cues
- Sofa: arms and cushion divisions
- Bed: mattress/pillow/bed-frame lines
- Cabinet: front divisions, door swing/hinge lines where relevant
- Appliances: recognizable plan outlines

### Basic 3D quality

- Lightweight
- Recognizable
- Correct proportions
- Major components visible
- Suitable for daily work and large projects

### Full 3D quality

- More refined geometry
- Component separation
- Suitable for interior views and export
- Still performance-controlled

## 18. Representation implementation contract

Prefer a managed variant/replacement architecture rather than keeping all heavy geometry permanently inside every loaded family.

Conceptually an asset may map to:

- `VZ_CH_001_2D`
- `VZ_CH_001_BASIC`
- `VZ_CH_001_FULL`

The logical Asset Instance remains stable while its physical representation changes.

Every switch must preserve:

- Location
- Rotation
- Level
- Host
- Width/Depth/Height
- Material assignments
- Asset ID
- Stable VIZORA instance identity
- User customization
- Relationship membership
- Manual Override

Switching representation must never return an asset to its original placement or project origin.

## 19. Dimensions and grips

- Dimensions must be editable in VIZORA Selected.
- Where practical, the same parameters must be editable in native Revit Properties/Edit Type.
- A dimension must appear once, not as duplicated stacked fields.
- Display units must follow Revit Project Units.
- A typical base-cabinet depth is approximately `600 mm / 24 in`.
- Stretchable assets must provide plan grips for appropriate dimensions.
- Numeric editing and grip editing must update the same authoritative parameter.
- Fully parametric assets may use free values unless constraints are required.
- Sculptural assets must use controlled ranges or approved types to avoid distortion.

Fully parametric candidates:

- Cabinets
- Desks
- Wardrobes
- Shelves
- Simple tables

Controlled-parametric candidates:

- Designer chairs
- Sofas
- Organic furniture

## 20. Material slots

Materials must be component-level and use existing Revit materials where possible.

Examples:

Chair:

- Frame
- Legs
- Seat
- Back
- Upholstery
- Accent

Sofa:

- Body
- Seat Cushion
- Back Cushion
- Legs
- Piping/Accent

Cabinet:

- Carcass
- Front
- Countertop
- Handle
- Plinth
- Interior
- Appliance Finish

---

# Part VI — Library, custom assets and shared content

## 21. Core Library strategy

Do not start with an uncontrolled huge library. First establish a correct and repeatable asset pipeline, then deliver the defined V1 content target below.

During internal development, begin with one high-quality representative asset per category. Expand only after that category's geometry, 2D graphics, thumbnail, dimensions and material system pass the visual and technical quality gate.

The first vertical slice uses one fully finished Chair. Later categories are added only after the asset contract is proven.

### 21.0.1 V1 Library quantity target

The planned V1 release target is **five approved assets for every active user-facing asset category** included in the agreed V1 catalog.

Examples of standard user-facing categories include:

- Dining Chairs
- Armchairs/Lounge Chairs
- Sofas
- Beds
- Desks
- Dining Tables
- Coffee Tables
- TV Units
- Wardrobes
- Shelves
- Refrigerators
- Ovens
- Cooktops
- Sinks
- Dishwashers
- Agreed basic lighting categories

The number five is a content-delivery target, not permission to clone one poor asset five times. The five assets must provide meaningful visual or functional variety.

For parametric systems such as cabinets, fillers and corner units, do not create five redundant families only to satisfy a count. Instead deliver:

- One reliable parametric system covering required module sizes and behaviors.
- All required functional unit types.
- Five approved visual style/front/handle collections where the category benefits from aesthetic choice.

Examples may include flat slab, shaker, framed, handleless and another approved style. The exact style names remain editable content metadata.

A category is V1-ready only when its promised five assets—or its approved parametric-system equivalent—pass the complete asset quality gate. Do not advertise incomplete or rejected items in the count.

### 21.0.2 Future Library expansion is user-directed

After the complete V1 candidate, Library quantity is expanded only when the user requests a content update.

Examples:

- Increase one chair category from five to fifteen assets.
- Add a new sofa style pack.
- Replace or improve thumbnails for one category.
- Add additional cabinet-front collections.
- Add a new appliance content pack.

Do not automatically bloat the Core Library simply because more assets can be generated.

When manifest/native-family schemas remain compatible, a Library expansion must be delivered as a versioned content-pack update without rebuilding unrelated plugin runtime modules. A plugin update is required only when the content needs a new runtime capability or schema contract.

Every Library update must report:

- Pack name and version.
- Categories affected.
- Assets added, replaced, deprecated or removed.
- Thumbnail/2D/3D QA result.
- Compatibility requirements.
- Download/install size.
- Rollback target.

Every Core asset must have:

- Valid manifest
- Real thumbnail
- Recognizable 2D Plan representation
- Basic 3D
- Full 3D when required by the phase
- Tested dimensions
- Tested materials
- Valid category/style metadata

### 21.1 Library visual-design contract

The Library is a major product surface, not a developer-only file list. It must look curated and architectural.

Asset cards must use a consistent VIZORA design system:

- The image is the dominant element.
- Clean spacing, typography and hierarchy.
- Asset name, category/style and essential dimensions remain readable.
- Small badges indicate available `2D Plan`, `Basic 3D` and `Full 3D` representations.
- Favorite control is visible but does not cover the asset.
- Source state is clear: Core, Custom or Shared.
- Missing or invalid content is reported explicitly and is not disguised as a finished asset.

Production Library cards must not rely on colored boxes, initials or generic placeholder icons as the final thumbnail.

### 21.2 Thumbnail standard

Core Library thumbnails must be generated from the actual approved asset using one controlled visual template.

Required thumbnail direction:

- Consistent three-quarter isometric/perspective view.
- Object centered with consistent framing and scale margin.
- Neutral light background compatible with the VIZORA UI.
- Soft controlled contact shadow or ambient grounding.
- Clean studio-style lighting.
- Correct material separation without noisy textures.
- No Revit selection outline, grids, levels, reference planes, UI chrome or crop artifacts.
- Same camera logic across one category.
- Transparent background may be stored as an additional source, but the UI result must remain consistent.
- Minimum working render size should be defined by the implementation; do not upscale a visibly poor thumbnail.

Recommended composition:

- Camera elevation approximately 25–35 degrees.
- Three-quarter horizontal angle appropriate to the asset category.
- Object occupies roughly 75–85 percent of the safe image area.
- Cabinet/appliance thumbnails must clearly show fronts, handles and depth.
- Seating thumbnails must clearly show seat, back, arms/legs and material separation.

The thumbnail pipeline must be deterministic and repeatable. Store the thumbnail recipe/version in metadata so a category can be regenerated after a visual-style change.

### 21.3 Library visual QA

An asset cannot be marked Library-ready until its review sheet passes:

- Thumbnail preview
- 2D Plan preview
- Basic 3D preview
- Full 3D preview, when required
- Dimensions and proportions
- Material slots
- Category/style/source metadata
- Naming and Asset ID
- File size and geometry budget

Generate a visual QA contact sheet or HTML/Markdown report for every content-pack candidate. The AI developer must inspect this report before asking the user to test the Library.

For the first asset in each category, user approval of the visual direction is required before producing the rest of that category. This prevents ten poor assets from being generated from an unapproved template.

### 21.4 Asset quality gate

Reject or quarantine an asset if:

- Its thumbnail is missing, misleading or inconsistent.
- Its 2D representation is only a generic box.
- Basic 3D does not visually identify the furniture type.
- Full 3D is merely a duplicate of Basic with no meaningful refinement.
- Required material slots do not affect the correct components.
- Dimensions distort the form outside allowed behavior.
- Geometry/file size exceeds the category budget without justification.
- The manifest and physical family disagree.

Do not count a rejected asset in advertised Library totals.

## 22. Custom Asset workflow

The first practical Custom Asset workflow registers an existing Revit family:

1. User selects a Revit `FamilyInstance`.
2. VIZORA captures family/type/category information.
3. User enters or confirms name, category and style.
4. User optionally assigns a thumbnail and reference image.
5. Front remains optional.
6. Smart Rotation remains OFF unless explicitly enabled.
7. VIZORA validates the registration.
8. Asset becomes available under Custom.

Do not ask for orientation/front on every import.

## 23. Reference Image Draft

A user may create a non-placeable draft from one or more reference images and basic dimensions. The draft becomes placeable only after a real compatible family/model is attached.

Automatic furniture generation from a photograph is a future phase, not part of the initial core. Do not imply that a draft is a completed 3D asset.

## 24. Shared/Team Library

- User can register one or more shared folders.
- Files are not deleted when a folder is removed from VIZORA settings.
- Shared catalogs are validated before merge.
- Duplicate Asset IDs produce explicit conflicts.
- External RFA files load on demand.
- Invalid assets are quarantined from the active catalog and reported.

### 24.1 Content packs must be independent from plugin code

Library content must be versioned separately from the plugin runtime wherever technically practical.

Define installable/updateable content packs such as:

- `Core-Seating`
- `Core-Tables`
- `Core-Bedroom`
- `Core-Kitchen`
- `Core-Appliances`
- `Core-Lighting`

Each pack owns its assets, manifests, thumbnails, QA report and pack version. The plugin consumes the published catalog contract.

This separation must allow:

- Replacing one bad thumbnail without rebuilding unrelated features.
- Updating one asset/category pack without rewriting Kitchen or Rooms code.
- Adding a new category through validated registry/content changes.
- Rolling back a content pack independently when compatible.
- Reporting exact runtime version plus content-pack versions in diagnostics.

Compatibility between plugin version, manifest schema, native family schema and content-pack version must be validated before activation.

---

# Part VII — Relationships and smart layouts

## 25. Relationship primitives

Provide reusable relationship concepts:

- Anchor
- Face Target
- Center Between
- Align
- Keep Distance
- Distribute
- Follow
- Rotate Toward

Every relationship operation must support:

- Preview
- Apply
- Disable/detach
- Manual Override
- Stable object IDs
- Conflict reporting

Do not perform continuous heavy relationship updates on `Idling`.

## 26. Smart Kitchen workflow

1. User selects one or more valid walls or edges.
2. VIZORA creates a Kitchen Zone.
3. User chooses Linear, L, U, Galley or Island layout.
4. User chooses required components.
5. Solver generates a preview.
6. UI displays modules, fillers, conflicts and unused space.
7. User applies the proposal.
8. Result remains manually editable.

Required component types:

- Base cabinets
- Wall cabinets
- Tall cabinets
- Refrigerator
- Sink
- Dishwasher
- Cooktop
- Oven
- Corner units
- Fillers
- Island modules

Example standard widths:

- 300 mm
- 450 mm
- 600 mm
- 750 mm
- 900 mm

Core rules:

- Dishwasher near sink
- Suitable sink/cooktop modules
- Sensible cooktop/refrigerator separation
- Correct corner handling
- Fillers near walls/obstructions
- No overlaps
- Prefer standard modules
- Fill usable wall space predictably
- Coordinate wall cabinets with the base run

## 27. Auto Reflow

When a key component such as a refrigerator is moved:

- Detect the changed available intervals.
- Recompute affected modules only.
- Preserve unaffected manual edits where possible.
- Generate a preview.
- Ask before applying.
- Do nothing automatically when Auto Reflow is OFF.

## 28. Smart Room presets

### Living Room

Examples:

- Sofa + coffee table + rug + TV
- L-shaped sofa + table + TV
- Sofa + two armchairs
- Conversation layout

Potential relationships:

- TV as anchor
- Sofa faces TV
- Coffee table centered between sofa and TV
- Rug centered around seating group

### Dining Room

- Dining table as anchor
- Chairs distributed around table
- Recalculate chair count/spacing when table size changes

### Bedroom

- Bed as anchor
- Nightstands left and right
- Rug centered with bed
- Bench at foot of bed

Room presets are editable starting points, not locked templates.

---

# Part VIII — Performance and reliability

## 29. Performance rules

- Do not preload every family at startup.
- Load assets and representations on demand.
- Lazy-load and cache thumbnails.
- Load Full 3D only when requested.
- Keep element scans scope-aware.
- Whole-project operations require preview counts, progress and cancellation where practical.
- Use a versioned cache.
- Do not perform expensive work on `Idling`.
- Keep the ExternalEvent queue observable and bounded.
- Do not regenerate an unchanged family repeatedly.
- Measure large-model behavior using defined performance fixtures.

## 30. File-lock and temporary-file safety

Family generation/loading must not fail because a temporary RFA is still used by another process.

Implement:

- Unique temporary working paths
- Explicit stream/document disposal
- Atomic publish into cache
- Per-asset generation locks
- Retry only for known transient file-lock cases
- Stale-lock detection
- Cancellation-safe cleanup
- No simultaneous writers for the same cache artifact

Never hide a file-lock exception behind a generic error.

## 31. Transaction safety

- Validate before opening a transaction when possible.
- Use the smallest practical transaction scope.
- Roll back failed operations completely.
- Do not leave half-created kitchen or room layouts.
- Return an operation result that reports created, updated, skipped and failed elements.
- Support deterministic retry when safe.

## 32. Project State

Use a versioned Project State with migrations.

Project-owned state may include:

- VIZORA instance registrations
- Current representations
- Custom dimensions/materials
- Relationships
- Manual overrides
- Custom presets
- Custom assets
- Optional front/orientation metadata

User-owned state must be separate:

- Favorites
- Recent assets
- UI preferences
- Shared-library paths
- Report destination

Never use display names as durable identifiers.

---

# Part IX — Testing and minimal user involvement

## 33. User-involvement policy

The user must not become the primary debugger.

Do not send a new installer after every small code change. Complete, test and internally review a coherent workspace/category milestone before asking the user to install it.

The intended user-testing frequency is:

- Exactly three planned main installation/test packages for the complete product roadmap described below
- Exactly one consolidated feedback submission from the user after each main stage
- At most one optional Final Acceptance Patch after Stage 3, and only when Stage 3 feedback contains a real release-blocking defect
- No small intermediate installers, experimental ZIPs or one-fix-at-a-time user builds between these stages

Before delivering any package, the AI developer must perform every test possible without the user's Revit installation.

The user should mainly:

1. Install one release candidate.
2. Open Revit 2027.
3. Follow a short, exact acceptance checklist.
4. Give final observations or a Support ZIP.

The plugin itself must guide this process through a built-in **VIZORA Test & Feedback Center**. The user must not manually reconstruct version information, capability lists or technical logs.

Internal development may use as many local/CI candidate builds as necessary, but those candidates are not sent to the user. The AI developer owns their debugging and consolidation.

## 34. Test pyramid

### Required automated tests

- Domain rule tests
- Application workflow tests
- Manifest parsing/validation
- Category registry tests
- Schema migration tests
- Representation state-transfer tests
- Kitchen solver tests
- Relationship tests
- Settings serialization tests
- Duplicate Asset ID tests
- Cache concurrency/file-lock tests where possible
- Error-code ownership tests
- Module dependency tests

### Required static/build checks

- Restore/build relevant .NET projects
- Treat warnings as errors
- Nullable-flow checks
- XAML compilation
- Event-handler existence
- Module registration validation
- JSON schema/catalog validation
- No forbidden project references
- No circular feature dependencies
- No placeholder command exposed as complete UI
- Installer-script syntax checks
- Package-content audit

### Revit-specific test distinction

Never call a static check a real Revit compile/runtime pass.

Use these exact labels:

- `STATIC CHECKS PASSED`
- `SOURCE BUILD PASSED` only when the actual required references were available
- `READY FOR REVIT TEST`
- `REVIT 2027 PASSED` only after evidence from the user's machine

## 35. Feature acceptance test specification

Every feature must define:

- Preconditions
- Exact user actions
- Expected visible result
- Expected Revit model result
- Expected log entries
- Failure recovery behavior
- Regression checks

Keep the user's test checklist short. Internal tests should cover edge cases before delivery.

## 36. Support package

When build, install or runtime fails, generate one Support ZIP containing:

- Product version
- Commit SHA
- Build environment summary
- Revit path/version detection
- Relevant logs
- Operation correlation IDs
- Feature/error codes
- Sanitized settings
- Catalog/schema validation report
- Module/version inventory

Do not include the user's entire Revit model unless they explicitly choose to attach it.

The report destination must be configurable by the user and remembered.

## 36.1 Built-in Test & Feedback Center

Create a dedicated Test & Feedback Center inside VIZORA, accessible from the Ribbon and Settings/Diagnostics. It must be usable by a non-programmer.

The Center is driven by release metadata generated from source, not by manually maintained UI text.

Every release candidate must include:

- `capabilities.json` — machine-readable list of capabilities included in this version.
- `release-test-plan.json` — exact guided tests for this candidate.
- `known-limitations.json` — honest list of incomplete or unsupported behavior.
- `module-versions.json` — runtime and feature/content-pack versions.
- User-facing release notes generated from the same source.

When opened, the Center must show:

- Installed VIZORA version.
- Source commit/build identity.
- Revit version.
- Active content-pack versions.
- Capabilities added or changed in this update.
- Capabilities expected to work now.
- Capabilities intentionally not available yet.
- Known limitations.
- Exact tests requested from the user for this milestone.
- Previous acceptance state, if available.

Do not advertise a capability that is not present in `capabilities.json` and connected to a real implementation.

## 36.2 Automated pre-test

Before showing manual steps, the Test Center must run safe automated checks such as:

- Module registration and version alignment.
- Required files/content packs present.
- Manifest and schema validity.
- Duplicate/missing Asset IDs.
- Thumbnail availability and dimensions.
- Representation variant availability.
- Writable cache/temp/report paths.
- Stale cache lock detection.
- Project State readability/migration readiness.
- ExternalEvent/gateway health where safely testable.
- Basic supported-selection detection.
- Optional non-destructive test-fixture checks.

Each check must produce Pass, Fail, Warning or Not Applicable with an owning Feature ID and error code.

Automated checks must not modify the user's production model unless the user explicitly starts a disposable sandbox test.

## 36.3 Guided Revit acceptance tests

Manual checks must be presented one at a time with:

- Test ID.
- Feature/workspace name.
- Why the test exists.
- Exact action, written briefly.
- Expected visible result.
- Expected Revit-model result.
- Buttons: `Pass`, `Fail`, `Skip / Not Applicable`.
- Optional comment field.
- Optional screenshot/file attachment.

The test list must be limited to the features changed in the current milestone plus a short set of critical regression checks. Do not make the user retest the entire product after every local patch.

The Center must remember progress if Revit is restarted.

## 36.4 Feedback package generated by the plugin

At the end of a test—or immediately after a failure—the user presses one button:

`Create VIZORA Feedback Package`

The plugin then creates a ZIP and a short readable summary containing:

- Version, commit/build identity and module/content-pack versions.
- Revit and operating-system environment summary.
- Automated pre-test results.
- Guided test results.
- User comments and explicitly attached screenshots/files.
- Relevant log window around failures.
- ErrorCode, FeatureId, TestId, OperationId and CorrelationId.
- Relevant Asset IDs and Revit Element IDs.
- Cache/file-lock state when relevant.
- Catalog/schema/module validation reports.
- Known limitations shipped with the release.
- A machine-readable `feedback.json`.
- A human-readable `FEEDBACK-SUMMARY.md`.

Privacy rules:

- Do not include the full Revit model by default.
- Do not include unrelated user files.
- Show exactly what will be included before creation.
- Allow the user to remove attachments.
- Sanitize personal paths where practical while retaining useful diagnostics.

The report folder is user-selectable and remembered.

## 36.5 Feedback-to-module routing

`feedback.json` must allow the next AI developer/session to route the issue directly to the responsible module.

Minimum routing fields:

- `featureId`
- `moduleId`
- `testId`
- `errorCode`
- `operationId`
- `correlationId`
- `assetId`
- `elementIds`
- `expectedBehavior`
- `observedBehavior`
- `reproductionSteps`
- `logs`
- `affectedVersions`

The repository must contain a machine-readable feature ownership map that resolves each `featureId`, `moduleId` and error-code prefix to:

- Owning source directories.
- Owning test directories.
- Public contracts.
- Declared dependencies.
- Related checklist IDs.

This is required so a Kitchen failure routes to Kitchen code/tests first instead of triggering an uncontrolled repository-wide rewrite.

---

# Part X — Progress management and checklist discipline

## 37. Single source of truth

Maintain these files from the first commit:

- `README.md`
- `PROJECT-SPEC.md`
- `ARCHITECTURE.md`
- `ROADMAP.md`
- `CHECKLIST.md`
- `STATUS.md`
- `CHANGELOG.md`
- `CURRENT-STATE.json`
- `AGENTS.md`
- `docs/decisions/`

`CURRENT-STATE.json` must be machine-readable and include:

- Current version
- Current phase
- Phase status
- Active development branch
- Last accepted release
- Last user-accepted phase
- Current source commit
- Schema versions
- Open blockers
- Next action

Also maintain:

- `FEATURE-MAP.json` — feature/module ownership, paths, contracts and dependencies.
- `CAPABILITY-MATRIX.json` — capability status and owning feature.
- `packaging/capabilities.json` — capabilities shipped in the current candidate.
- `packaging/release-test-plan.json` — generated user acceptance plan.
- `packaging/known-limitations.json` — explicit candidate limitations.

## 38. Checklist structure

Use a hierarchical checklist with stable IDs.

Example:

- `FND-001` solution structure
- `FND-002` module registry
- `FND-003` Revit gateway
- `LIB-001` manifest contract
- `LIB-002` category registry
- `REP-001` representation state contract
- `KIT-001` kitchen zone model
- `KIT-002` linear solver
- `ROM-001` room anchor model
- `EXP-001` export profile contract

Each item must record:

- Status: Not Started / In Progress / Source Complete / Ready for Revit Test / Passed / Blocked
- Owner module
- Dependencies
- Relevant tests
- Commit/PR
- Acceptance evidence
- Known limitations

Do not mark an item complete merely because UI exists.

## 39. Progress report format

At every coherent milestone, update `STATUS.md` with:

1. Current phase and version
2. Completed since last milestone
3. Tests actually run
4. Tests not possible in the current environment
5. Known issues
6. Files/modules changed
7. User test required, if any
8. Exact next action

Do not use vague statements such as “everything is done.”

The progress report must separately state:

- Runtime/plugin progress.
- UI progress.
- Library-content progress.
- Thumbnail/visual-QA progress.
- Automated-test progress.
- Real-Revit acceptance progress.

A feature cannot be reported as complete when its runtime exists but its required content, thumbnail, UI connection or acceptance test is missing.

## 40. Decision records

For meaningful architecture changes, add a short ADR under `docs/decisions/`.

Examples:

- Why representation uses replacement variants
- Where project state is stored
- How feature modules communicate
- Why no heavy `Idling` behavior exists
- How cache locking works

This prevents future AI sessions from undoing important decisions.

---

# Part XI — Git and release workflow

## 41. Repository policy

This is a clean implementation. Do not copy old source code unless the user later provides a specific file and explicitly requests review or migration.

Keep `main` coherent and recoverable.

Use feature/candidate branches for incomplete work. Record the active branch in `CURRENT-STATE.json`.

After every coherent coding batch:

1. Run available tests.
2. Update checklist/status.
3. Commit the coherent change.
4. Sync GitHub.
5. Only then begin the next batch.

Do not let completed work exist only in an AI session or temporary container.

### 41.1 Targeted-update contract

Every future change request must be mapped before editing:

1. Identify Feature ID and owning Module ID.
2. Read the module's public contract, checklist and tests.
3. List the files expected to change.
4. List declared dependencies that may legitimately change.
5. Do not edit unrelated modules unless a contract change makes it necessary.
6. If a shared contract changes, document impact and run dependent contract/regression tests.
7. Update `FEATURE-MAP.json` only when ownership or boundaries change.

For a request such as “change the Library card design,” the default change scope is UI theme/shared card components plus visual tests—not Kitchen or Room logic.

For a request such as “change cabinet category structure,” the default change scope is Category Registry, affected content pack and relevant filters/tests—not a rewrite of the Dockable Panel or Representation engine.

For a Kitchen runtime failure, begin with the `KIT-*` diagnostic route, Kitchen module and its declared Revit/cache dependencies.

Every pull request/commit batch must include an impact statement:

- Target feature/module.
- Files changed.
- Reason each outside-module file changed.
- Tests run.
- Unchanged critical modules.

## 42. Versioning

Use SemVer:

- Patch: bug fix
- Minor: completed feature phase
- Major: stable product generation with breaking-contract control

Never overwrite a previously delivered release.

## 43. Release candidate gate

Before producing an installer/ZIP:

- Confirm version consistency
- Confirm source/commit alignment
- Validate manifests/catalogs/schemas
- Run module-boundary checks
- Run automated tests
- Build available projects
- Verify XAML and event handlers
- Check nullable warnings
- Audit package contents
- Verify installer scripts
- Verify ZIP opens and contains expected files
- Generate SHA256

Report:

- Version
- Source commit SHA
- Source tree SHA, when available
- ZIP SHA256
- Tests run and results
- Tests still requiring real Revit

The ZIP must be generated from the same source snapshot represented by GitHub.

## 44. Build and installation experience

Deliver one clear package containing:

- A simple `START-VIZORA.bat` or equivalent entry point
- PowerShell 5.1-compatible build/install scripts
- Non-default Revit 2027 path detection/selection
- .NET SDK verification
- RevitAPI reference verification
- Safe cleanup of previous build output
- Clear success/failure result
- Automatic Support ZIP on failure

Do not require the user to edit source or scripts manually.

---

# Part XII — Implementation phases

## 44.1 User delivery contract: three main installs plus one optional patch

Internal implementation phases do not equal user installations. The work may contain many internal commits, tests and candidates, but the user receives only the following three planned main packages.

### Main Install 1 — Stage 1: Core System and Visual Standard

Purpose: prove that the foundation, modular architecture and primary asset workflow are genuinely correct before smart-layout expansion.

Must include:

- Clean architecture and feature-module boundaries
- Ribbon and Dockable Panel shell
- Library workspace foundation
- Approved VIZORA UI/theme system
- Professional asset-card design
- Deterministic isometric/perspective thumbnail standard
- One complete high-quality vertical-slice asset
- A small curated starter content set only after the representative visual standard is approved internally
- Selected workspace
- Editable dimensions and component materials
- Revit Properties integration where practical
- Supported plan grips
- `2D Plan / Basic 3D / Full 3D`
- Transform/state preservation during representation changes
- Scene representation scope and basic presets
- Logging, error ownership and Support Package
- Test & Feedback Center

Before delivery, all possible automated/static/build/package checks must pass. The package status is `STAGE 1 — READY FOR REVIT TEST`, not PASSED.

The user installs once, completes the guided Stage 1 tests and creates one consolidated Feedback Package.

Stage 1 feedback is triaged and fixed internally. Its fixes are included and regression-tested in Main Install 2. Do not send a separate Stage 1 hotfix installer.

### Main Install 2 — Stage 2: Smart Design Systems

Purpose: deliver the smart interior-design workflows on top of the corrected Stage 1 foundation.

Must include:

- Every accepted Stage 1 fix
- Kitchen component system
- Cabinets, fillers, corners, handles and initial appliances
- Correct cabinet/appliance 2D graphics
- Kitchen dimensions, materials and grips
- Kitchen Zone selection/validation
- Linear, L, U, Galley and Island workflows according to internal completion gates
- Preview, conflicts, Apply and rollback
- Auto Reflow with Ask Before Update
- Relationship primitives
- Living, Dining and Bedroom preset workflows
- Manual Override protection
- Custom Asset registration from selected Revit family
- Expanded but still quality-gated Library content

Before delivery, the AI developer must verify that Stage 1 fixes did not regress and that all possible Stage 2 checks pass.

The user installs once, completes the guided Stage 2 tests and creates one consolidated Feedback Package.

Stage 2 feedback is triaged and fixed internally. Its fixes are included and regression-tested in Main Install 3. Do not send a separate Stage 2 hotfix installer.

### Main Install 3 — Stage 3: Complete Release Candidate

Purpose: deliver the complete planned V1 candidate with all prior fixes, final content, performance and release polish.

Must include:

- Every accepted Stage 1 and Stage 2 fix
- Complete agreed V1 Library/content-pack scope
- Final approved thumbnails and visual QA reports
- Shared/Team Library
- External RFA validation and load-on-demand
- Reference Image Draft workflow
- Export-preparation profiles
- Performance and large-project safeguards
- Final settings, diagnostics and configurable report path
- Cache/file-lock safeguards
- Full regression pass across Stage 1 and Stage 2
- Release documentation and known limitations
- Version/commit/tree/package hashes

The user installs once, completes the guided Stage 3 tests and creates one consolidated Final Feedback Package.

### Optional Final Acceptance Patch

The optional patch is permitted only if Stage 3 feedback identifies one or more of the following:

- Installation or load failure
- Crash, data loss or project corruption risk
- A required V1 capability that does not function as specified
- A serious representation/state-preservation defect
- A serious Kitchen/Room runtime defect
- A blocking Library/content-pack defect
- Another issue that prevents V1 acceptance

The patch must:

- Contain only Stage 3 acceptance fixes and required regressions
- Avoid new product features
- Use a SemVer patch increment
- Include a change-impact report mapped to Feature/Module IDs
- Run focused regression tests plus all critical release checks
- Be delivered as one consolidated package, not multiple micro-patches

Minor preferences, new ideas and non-blocking improvements discovered after Stage 3 are added to the post-V1 backlog and do not trigger the optional patch unless the user explicitly changes the scope.

Therefore the normal user involvement is three installs. The maximum planned involvement is three main installs plus one optional Final Acceptance Patch.

## 44.2 One-feedback rule per stage

For each main stage:

1. The Test & Feedback Center presents only that stage's changed capabilities and critical regressions.
2. The user completes testing over their chosen testing period.
3. The Center stores each Pass/Fail/comment without requiring immediate submission.
4. The user creates one final consolidated Feedback Package for the stage.
5. The AI developer groups all findings by Feature ID, severity and root cause.
6. The AI developer makes one coherent correction batch for the next delivery.

Do not ask the user to install again because one item was fixed. Do not ask for separate log files when the Feedback Package already contains them.

## 44.3 Stage gate before sending an installer

No main-stage package may be sent to the user until:

- All scope items for that stage are Source Complete or explicitly documented as excluded
- Automated tests pass
- Architecture/dependency checks pass
- Available build checks pass
- Package audit passes
- Capability metadata matches actual implementation
- Guided test plan is generated
- Known limitations are written honestly
- Visual QA passes for all included Library content
- GitHub source and package snapshot match
- SHA256 is generated

If the internal gate fails, continue working internally. Do not consume one of the user's three installation cycles with a package already known to be incomplete.

## 45. Phase 0 — Governance and clean foundation

Deliverables:

- Repository structure
- Solution/layer boundaries
- Feature/module registry
- Dependency tests
- Documentation and checklists
- Logging/error contracts
- Revit operation gateway
- Ribbon shell
- Dockable Panel shell
- Build/install/support pipeline
- Test & Feedback Center shell
- Capability/release-test metadata pipeline
- `FEATURE-MAP.json` and targeted-diagnostic routing

Acceptance condition:

- The plugin builds/installs/loads on the user's Revit 2027 machine.
- The panel opens.
- Diagnostics identify environment and version.
- No product feature is falsely presented as complete.

## 46. Phase 1 — One complete vertical asset slice

Implement one credible Chair end-to-end:

- Manifest
- Category/style metadata
- Thumbnail
- 2D Plan graphics
- Basic 3D
- Full 3D
- Placement
- Selection
- Editable dimensions
- Supported grips
- Material slots
- Representation switching
- Transform/state preservation
- Logging and tests

Do not add many assets before this slice passes.

Acceptance condition:

- One real asset proves the entire architecture.
- Moving the Chair and switching representation preserves its position and rotation.
- Dimensions and materials are actually editable.
- 2D is a meaningful furniture drawing.

## 47. Phase 2 — Library workspace

Deliver:

- Asset browser
- Search
- Category/style/source filters
- Favorites
- Recent
- Small validated Core catalog
- Custom Asset registration from selected Revit family
- Approved VIZORA asset-card design system
- Deterministic isometric/perspective thumbnail pipeline
- Visual QA report/contact sheet
- Independent versioned content packs

The Library workspace is validated internally and delivered as part of Main Install 1. Do not create an additional Library-only installer.

Before generating multiple assets for a category, the user first approves one complete representative asset, its thumbnail, 2D drawing and 3D quality. Only then may the same approved system be expanded to the category.

## 48. Phase 3 — Selection and Representation workspaces

Deliver:

- Reliable supported-asset selection state
- Native property mapping where practical
- Selected/View/Project representation scopes
- Preview counts
- Representation presets
- State preservation and rollback

## 49. Phase 4 — Kitchen components

Before Smart Kitchen, complete the actual components:

- Base cabinet
- Wall cabinet
- Tall cabinet
- Corner unit
- Filler
- Countertop behavior
- Replaceable handles
- Sink
- Refrigerator
- Oven
- Cooktop variants
- Dishwasher
- Correct 2D graphics
- Dimensions/grips/materials

## 50. Phase 5 — Smart Kitchen

Implement in this order:

1. Kitchen Zone validation
2. Linear layout
3. Preview/conflicts
4. Apply/rollback
5. L layout
6. U layout
7. Galley
8. Island
9. Auto Reflow

Do not implement advanced layouts before the Linear path is reliable.

## 51. Phase 6 — Relationships and Rooms

Implement reusable relationship primitives before room-specific presets.

Then add:

- Dining
- Bedroom
- Living

Each must respect Ask Before Update and Manual Override.

## 52. Phase 7 — Export preparation

Deliver non-destructive preparation profiles for:

- Exterior Render
- Twinmotion Working
- Unreal Final
- Documentation

Display proposed representation/category changes before Apply.

## 53. Phase 8 — Shared Library and expansion

- Shared/Team folders
- External RFA validation/load-on-demand
- Catalog conflict handling
- Expanded high-quality library
- Performance profiling on large projects

## 54. Phase 9 — Future work

- Reference-based furniture generation
- Larger curated catalog
- Revit 2026/2025 backports
- Optional cloud distribution/update service

These are not MVP requirements.

---

# Part XIII — Definition of Done

## 55. A feature is complete only when all are true

1. Requirement is documented.
2. Owning module is explicit.
3. Public contract is defined.
4. Implementation respects dependency boundaries.
5. UI is connected to real backend behavior.
6. Error handling and rollback exist.
7. Automated tests exist and pass.
8. Static/build checks are reported honestly.
9. Checklist and status are updated.
10. GitHub contains the coherent source snapshot.
11. Release package matches that snapshot.
12. Real Revit acceptance is completed when required.
13. User approves the milestone.
14. Shipped capability and guided-test metadata are accurate.
15. Required Library visual/content assets pass their visual QA gate.

A button, view, placeholder message, mock catalog or class skeleton is not a completed feature.

## 56. Forbidden regressions and implementation shortcuts

Do not:

- Build a monolithic `VizoraPane.xaml.cs` containing all features.
- Put business logic in UI event handlers.
- Hide unfinished capabilities behind apparently active buttons.
- Use generic boxes as final furniture.
- Reset position when switching representation.
- Duplicate dimension controls.
- Force 2D/3D based on view type.
- Require Front for every custom asset.
- Enable Smart Rotation by default.
- Run heavy family work on `Idling`.
- Continuously move relationship followers on `Idling`.
- catch and ignore exceptions.
- use generic success messages without verifying model changes.
- call static validation a Revit runtime pass.
- send repeated untested installers to the user.
- rewrite unrelated modules to fix a feature-local error.
- ship poor or placeholder thumbnails as finished Library content.
- make the user manually collect version, log and module information for feedback.
- ask the user to repeat every historical test after a small module-local patch.

---

# Part XIV — First actions for the AI developer

## 57. Do this first

Before implementing furniture, kitchens or room presets:

1. Create the repository structure and solution architecture described here.
2. Create the governance files and machine-readable project state.
3. Create the feature ownership/dependency map.
4. Create the full hierarchical checklist with stable IDs.
5. Create the test projects and architecture-boundary checks.
6. Implement logging, error-code routing and Support ZIP contracts.
7. Implement `FEATURE-MAP.json`, capability metadata and release-test-plan generation.
8. Implement the Test & Feedback Center shell and one-button Feedback Package flow.
9. Implement the Revit gateway, Ribbon shell and Dockable Panel shell.
10. Create the build/install pipeline for Revit 2027.
11. Run all possible non-Revit checks.
12. Continue through the complete Stage 1 vertical slice, then prepare Main Install 1. Do not send a Foundation-only installer before the Stage 1 delivery gate is complete.

Do not start Phase 1 until Phase 0 is coherent and its available checks pass.

## 58. Required first response/report

After reading this brief, produce and commit:

- Proposed repository tree
- Module dependency map
- Feature ownership table
- Phase checklist with stable IDs
- Risk register
- Phase 0 implementation plan
- Test strategy
- Release/acceptance strategy

Then start Phase 0 without asking the user to repeat requirements already contained in this document.

If a decision is genuinely missing and changes the fundamental product behavior, record the options and ask one concise question. Otherwise use the defaults in this brief and continue.

---

# Core operating principle

VIZORA must grow as a set of independently maintainable capabilities, not as one accumulating code-behind file. The architecture, tests, progress system and release discipline are part of the product—not optional documentation.

The user should evaluate complete, internally checked milestones. The user must not be used as the substitute for automated tests, architecture review or basic debugging.

---

# Part XV — Lead architect product recommendations

The following recommendations are part of the project direction. They exist to prevent the implementation from technically satisfying individual requests while producing a weak or difficult product.

## 59. Treat VIZORA as two coordinated products

VIZORA should be designed as:

1. A stable **Runtime/Product Engine**: Revit integration, UI, placement, representation, smart systems, diagnostics and updates.
2. A separately versioned **Content Platform**: asset manifests, Revit families, 2D graphics, thumbnails, materials and content packs.

This separation is essential. Library growth should not destabilize Kitchen logic, and a Kitchen bug fix should not require repackaging every sofa.

## 60. Prove one golden asset before scaling a category

For each category, create one `Golden Asset` first. It defines:

- Correct proportions and visual quality.
- 2D Plan drawing language.
- Basic/Full representation difference.
- Parameter and grip behavior.
- Material-slot design.
- Thumbnail camera/lighting/framing.
- Geometry and file-size budget.

Only after that Golden Asset is approved internally—and visually by the user when first establishing the category direction—may the remaining four V1 assets be produced.

This is more reliable than producing five assets and discovering that all five use the wrong visual standard.

## 61. Make smart systems explainable, reversible and local

Kitchen and Room automation should never behave like an unexplained black box.

Every Preview should explain:

- What will be created.
- What will move.
- What will be replaced.
- Which rule caused each decision.
- Which conflicts remain.
- Which manual edits will be preserved.

Apply must be transactional and reversible. Reflow should recompute the affected run/group rather than rebuilding an entire floor.

## 62. Prioritize project safety over automation

The highest-priority quality order should be:

1. No project corruption or data loss.
2. Correct placement and state preservation.
3. Reliable dimensions/materials/representation.
4. Clear failure and rollback.
5. Performance.
6. Smart automation.
7. Library quantity.

An advanced automatic layout that occasionally corrupts state is less valuable than a smaller reliable system.

## 63. Build a contextual UI, not a wall of controls

The panel should expose only the controls relevant to the current workspace and selection.

- Library focuses on discovery and placement.
- Selected focuses on editing the selected asset.
- Kitchen focuses on zone, rules, preview and apply.
- Rooms focuses on anchors and presets.
- Export focuses on preparation profiles.

Advanced settings should remain available but collapsed by default. This keeps VIZORA usable even as capabilities grow.

## 64. Add visible health and performance indicators

VIZORA should include a lightweight Project/Scene Health summary, preferably inside Diagnostics rather than as a permanent main workspace.

Useful indicators:

- Number of registered VIZORA instances.
- Representation counts.
- Missing/broken assets.
- Invalid manifests or duplicate IDs.
- Outdated content packs.
- Cache size/state.
- Estimated heavy Full 3D usage.
- Relationship or Kitchen groups with unresolved conflicts.

Do not continuously scan the whole model. Refresh on demand or after known VIZORA operations.

## 65. Define and enforce performance budgets

Each asset/category should have measurable budgets for:

- RFA/file size.
- Geometry complexity.
- Thumbnail size.
- Load/generation time.
- Representation-switch time.

Each feature should have representative stress fixtures. The release report should identify performance regressions rather than relying on subjective statements such as “lightweight.”

## 66. Use golden test projects and visual baselines

Maintain small controlled Revit test projects for:

- Single asset editing.
- Representation switching after move/rotate/resize.
- Kitchen wall layouts and obstacles.
- Room relationships.
- Large repeated furniture counts.
- Export preparation.

Where technically practical, also maintain approved reference images/contact sheets for Library cards, thumbnails and plan symbols. Visual regressions must be reviewable before a user installer is produced.

## 67. Preserve compatibility and rollback

- Project State, manifests, native families and content packs must each have explicit schema/version contracts.
- Migrations must be tested against older saved fixtures.
- Content updates must support rollback to the previous compatible pack.
- Cache updates must publish atomically.
- A failed update must not break already placed project instances.

## 68. Keep the initial product local-first

The recommended first stable generation is local-first:

- No mandatory cloud account.
- No dependency on a server for basic Library/placement behavior.
- Shared folders are sufficient for the first Team Library.
- Feedback packages are created locally and shared by the user.

Cloud distribution, accounts and marketplace functionality can be considered only after the local product is stable.

## 69. Keep generated/reference-based furniture out of the critical path

Reference-image furniture generation is attractive but should not block V1. First make registration of real Revit families, representation switching, content packs and quality control dependable.

When generation is introduced, label approximate geometry honestly and require review before it enters the active Library.

## 70. Avoid unlicensed commercial replicas

Core assets should use original/generic design language or properly licensed sources. Do not present approximate copies of recognizable commercial products as official branded products.

Store source/license/provenance metadata for Library content where applicable.

## 71. Make updates precise and recoverable

The long-term update system should support separate channels for:

- Plugin runtime updates.
- Content-pack updates.
- Thumbnail-only refreshes.
- Schema migrations.

Before applying an update, show version, size, affected modules/packs and rollback availability. Never silently replace project-owned custom content.

## 72. Recommended V1 success criteria

V1 should be considered successful when:

- The three-stage delivery is accepted, with an optional final patch only if needed.
- All advertised workspaces perform real operations.
- No placeholder buttons are presented as complete features.
- Library visuals look intentionally designed and consistent.
- Every active standard category contains five approved assets or an approved parametric equivalent.
- Assets retain transform, dimensions, materials and identity across representation changes.
- Kitchen and Room actions are previewable, explainable and reversible.
- The user can create one Feedback Package without manually collecting technical data.
- A module-local issue can be located and patched without unrelated rewrites.
- Large-project behavior remains within documented performance targets.
