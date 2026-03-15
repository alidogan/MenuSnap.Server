<!--
SYNC IMPACT REPORT
==================
Version change: [TEMPLATE] → 1.0.0 (initial ratification, 2026-03-15)
Version change: 1.0.0 → 1.0.1 (2026-03-15 — PATCH: replace Mapster with manual static mapper classes)
Version change: 1.0.1 → 1.0.2 (2026-03-15 — PATCH: expand frontend testing to Vitest + RTL + Playwright)
Version change: 1.0.2 → 1.0.3 (2026-03-15 — PATCH: rename frontend repo MenuSnap.Web.Order → MenuSnap.Web.Portal)
Version change: 1.0.3 → 1.1.0 (2026-03-15 — MINOR: add Principle VII for MenuSnap.Web.Order (Next.js, performance-first))
Version change: 1.1.0 → 1.2.0 (2026-03-15 — MINOR: SignalR notifications, MinIO/S3 storage, frontend vertical slices)
Version change: 1.2.0 → 1.2.1 (2026-03-15 — PATCH: MinIO in Docker Compose list, @microsoft/signalr in frontend stacks, Lighthouse CI gate, i18n for Web.Order)

v1.2.0 changes:
  - Modified principle: VI. Frontend Architecture Consistency
      → Added ProCC-React vertical slice directory convention (core/shared/features/routes)
  - Modified principle: VII. Order App Performance-First
      → Added same frontend vertical slice convention applies
  - Modified tech stack (backend): added SignalR row, added MinIO/S3 row
Follow-up TODOs:
  - TODO(RATIFICATION_DATE): Confirm exact project inception date if different from 2026-03-15
  - TODO(MENUSNAP_MODULES): Define full module list beyond Menu/Order/Tenant/Identity as domain expands
-->

# MenuSnap Constitution

## Core Principles

### I. Modular Monolith Architecture

The system MUST be organized as a modular monolith following the EshopModularMonoliths pattern.
Each business domain (e.g., Menu, Order, Tenant, Identity) MUST be a self-contained module with:
- Its own `DbContext` and EF Core migrations
- Its own DI registration extension (`Add<Module>Module` / `Use<Module>Module`)
- Its own `Contracts` project for types shared across module boundaries

Modules MUST NOT reference each other's internal classes directly. Inter-module communication
MUST go through the `Contracts` project of the producing module or through the internal messaging
bus (MassTransit). The Bootstrapper (`Api` project) is the sole composition root that wires all
modules together.

**Rationale**: Enforces bounded contexts and prepares for potential future decomposition into
microservices without requiring a full rewrite.

### II. Vertical Slice Feature Organization

Within each module, features MUST be organized as vertical slices — one folder per use case under
`<Module>/Features/<FeatureName>/`. Each slice MUST contain its own:
- Command or Query record (MediatR)
- Handler
- FluentValidation Validator
- Carter endpoint (or minimal API registration)
- `<FeatureName>Mapper.cs` — a `static` class with explicit `ToEntity`, `ToDto`, and where
  applicable `ToEntityForCreate` methods; no auto-mapping libraries (e.g. Mapster, AutoMapper)

Mapper classes MUST be pure static methods with no dependencies. All property assignments MUST
be explicit; `ToEntityForCreate` MUST assign a `Guid.CreateVersion7()` when the incoming ID is
null or empty and MUST set `CreatedAt = DateTime.UtcNow`.

No horizontal layers (no global `Services/`, `Repositories/`, or `Controllers/` folders within a
module). Shared infrastructure (interceptors, behaviors, base classes) lives in `Shared`.

**Rationale**: Keeps all logic for a use case co-located, minimizes cross-cutting merge conflicts,
and makes it trivial to delete or replace a feature without touching unrelated code.

### III. Domain-Driven Design (NON-NEGOTIABLE)

Domain models MUST be rich entities, not anemic data bags. Specifically:
- Entities and Aggregates MUST inherit from the shared DDD base classes (`Entity`, `Aggregate`).
- Aggregates MUST encapsulate all invariant-enforcement logic; no invariant checks in handlers.
- Domain events MUST be raised inside aggregates and dispatched via the `DispatchDomainEventsInterceptor`.
- Value objects MUST be used for concepts with no identity (e.g., `Address`, `Money`, `Slug`).
- Persistence concerns (EF Core mappings) MUST be in separate `Data/` sub-folders; domain models
  MUST NOT contain EF Core attributes.

**Rationale**: Protects business rules at the correct layer and prevents the codebase from
degrading into a CRUD service as complexity grows.

### IV. Test-First Development

Integration tests MUST be written before the feature implementation for all API endpoints.
Unit tests MUST be written for all domain logic (aggregate methods, value objects, validators).
The Red-Green-Refactor cycle MUST be followed:

1. Write test → confirm it fails (Red)
2. Implement minimum code to pass (Green)
3. Refactor without breaking tests (Refactor)

Test projects MUST mirror the module structure. Integration tests MUST use a real PostgreSQL
instance (Testcontainers) and a real in-memory MassTransit bus.

**Rationale**: Tests serve as executable specifications and the primary safety net for the
modular boundaries. Skipping tests for "speed" is not acceptable.

### V. Structured Observability

All modules MUST use Serilog with structured (key-value) logging, configured at the Bootstrapper
level. The following MUST be instrumented:
- Incoming HTTP requests (via ASP.NET Core middleware or Serilog request logging)
- MediatR pipeline — a `LoggingBehavior` MUST log command/query name, duration, and outcome
- Domain event dispatch — log event type and aggregate ID
- Unhandled exceptions — log full exception context before returning a ProblemDetails response

In development, Seq MUST be the log sink. In production, the sink MUST be configurable via
`appsettings.{Environment}.json`.

**Rationale**: Distributed tracing and log correlation are essential for debugging a modular
system where a single request crosses multiple module boundaries.

### VI. Frontend Architecture Consistency

The frontend (`MenuSnap.Web.Portal`) MUST follow the ProCC-React patterns:
- **Routing**: TanStack Router with file-based route generation — no manual route registration.
- **Server state**: TanStack Query — no ad-hoc `useEffect` fetching.
- **Client-only state**: Zustand stores — no prop drilling beyond two levels.
- **UI components**: PrimeReact — no mixing of competing component libraries.
- **Validation**: Zod schemas + TanStack Form — no uncontrolled forms.
- **HTTP**: Axios with a shared client instance (base URL, auth interceptor).
- **i18n**: react-i18next — all user-visible strings MUST be translation keys.
- **Build**: Vite + Bun — no npm or yarn.
- **Testing** (three layers, all MUST be present):
  - Unit / logic: Vitest — pure functions, hooks, store logic, mapper classes.
  - Component: React Testing Library (RTL) on top of Vitest — render behavior, user interactions,
    accessibility assertions; no snapshot-only tests.
  - E2E: Playwright — full browser flows against a running dev server; no DOM internals.

**Directory structure** — vertical slices mirroring the ProCC-React convention:

```
src/
├── core/               # Cross-cutting framework concerns: auth, config, i18n, layouts, navigation
├── shared/             # Global primitives used by 3+ features: api client, components,
│                       # hooks, stores, models, utils, themes
├── features/
│   └── <feature>/
│       ├── hooks/          # Feature-level hooks (columns, mutations, edit state)
│       ├── views/          # Page-level view components (the "screen")
│       └── shared/         # Intra-feature reusables
│           ├── api/        # Axios/Query functions for this feature
│           ├── components/ # Feature-specific UI (modals, cards, forms)
│           ├── hooks/      # Query / mutation hooks shared within the feature
│           └── types/      # TypeScript interfaces for this feature
└── routes/             # TanStack Router file-based routes — thin wrappers only
```

Rules:
- Route files MUST only render the feature's view component; no business logic in route files.
- Cross-feature navigation MUST go through routes, not direct component imports.
- A primitive belongs in `src/shared/` only if 3 or more features use it; otherwise it stays
  inside the owning feature's `shared/` sub-folder.
- No barrel `index.ts` re-exports that cross feature boundaries.

**Rationale**: Consistency with the established ProCC-React baseline reduces onboarding time and
ensures tooling, testing, and CI configurations are reusable across both projects.

### VII. Order App Performance-First (Next.js)

`MenuSnap.Web.Order` is the customer-facing ordering app and MUST be built with Next.js for
server-side rendering, static generation, and Core Web Vitals compliance. Its constraints:

- **Framework**: Next.js (App Router) — no Pages Router; no client-only SPA patterns for
  routes that can be rendered on the server.
- **Rendering strategy**: Server Components by default; `"use client"` MUST be limited to
  interactive leaf nodes (forms, carousels, live updates). Justify every `"use client"` boundary.
- **Data fetching**: Next.js `fetch` with caching/revalidation in Server Components; TanStack
  Query only for client-side mutations or real-time polling where RSC is insufficient.
- **Styling**: Tailwind CSS v4 — consistent with `MenuSnap.Web.Portal`; no CSS-in-JS.
- **Images**: Next.js `<Image>` component — no raw `<img>` tags; all images MUST be optimized.
- **Performance gates** (MUST pass before merge to `main`):
  - Lighthouse Performance score ≥ 90 on mobile for all customer-facing routes.
  - Largest Contentful Paint (LCP) < 2.5 s on simulated 4G.
- **Testing**: Vitest + React Testing Library for unit/component; Playwright for E2E.
- **Auth**: Keycloak OIDC via `next-auth` or equivalent NextAuth.js adapter — no custom sessions.
- **i18n**: react-i18next — all user-visible strings MUST be translation keys; locale files
  MUST be kept separate from `MenuSnap.Web.Portal` (no shared translation bundles).
- **Directory structure**: Same vertical slice convention as Principle VI applies
  (`core/`, `shared/`, `features/<feature>/{hooks,views,shared/{api,components,hooks,types}}`,
  `app/` routes as thin wrappers). No business logic in Next.js route/page files.

`MenuSnap.Web.Order` and `MenuSnap.Web.Portal` MUST NOT share a runtime bundle. Shared UI
primitives (if any) MUST live in a dedicated `packages/ui` workspace package.

**Rationale**: The ordering app is customer-facing and directly impacts conversion. Server-side
rendering eliminates JavaScript parse cost on low-end devices and improves SEO for menu pages.

## Technology Stack

### Backend

| Concern | Choice | Notes |
|---|---|---|
| Runtime | .NET 10 | Target framework `net10.0` in all `.csproj` files |
| API endpoints | Carter | Minimal API module registration |
| CQRS / Mediator | MediatR | Commands, Queries, Domain Event handlers |
| Validation | FluentValidation | Registered via `ValidationBehavior` in MediatR pipeline |
| Object mapping | Manual mapping | Static `<Feature>Mapper` classes per slice; no auto-mapping libraries |
| ORM | EF Core (Npgsql) | One `DbContext` per module; code-first migrations |
| Database | PostgreSQL | One logical DB; schema-per-module via EF migrations |
| Caching | Redis (`StackExchange.Redis`) | Distributed cache for read-heavy queries |
| Messaging | MassTransit | In-process by default; RabbitMQ transport for production |
| Auth | Keycloak (`Keycloak.AuthServices`) | OIDC; no custom JWT implementation |
| Real-time / notifications | SignalR | Hub per bounded context; clients subscribe to typed hubs |
| File / image storage | MinIO (S3-compatible) | `AWSSDK.S3` or `Minio` SDK; one bucket policy per module |
| DI scanning | Scrutor | Assembly scanning for marker interfaces |
| Logging | Serilog + Seq (dev) | Structured; no `Console.WriteLine` |
| Containerization | Docker Compose | `docker-compose.yml` at solution root |

### Frontend — Portal (`MenuSnap.Web.Portal`)

| Concern | Choice |
|---|---|
| Language | TypeScript 5.x |
| Framework | React 19 |
| Build | Vite + Bun |
| Routing | TanStack Router (file-based) |
| Server state | TanStack Query v5 |
| Client state | Zustand v5 |
| UI library | PrimeReact v10 + Tailwind CSS v4 |
| Forms | TanStack Form + Zod |
| HTTP | Axios |
| Real-time | `@microsoft/signalr` |
| i18n | react-i18next |
| Unit / logic tests | Vitest |
| Component tests | React Testing Library + Vitest |
| E2E tests | Playwright |

### Frontend — Order App (`MenuSnap.Web.Order`)

| Concern | Choice |
|---|---|
| Language | TypeScript 5.x |
| Framework | Next.js (App Router) |
| Rendering | Server Components default; `"use client"` at leaf nodes only |
| Styling | Tailwind CSS v4 |
| Server state (client) | TanStack Query v5 (mutations / real-time only) |
| Forms | TanStack Form + Zod |
| Auth | NextAuth.js + Keycloak OIDC adapter |
| Real-time | `@microsoft/signalr` |
| Images | Next.js `<Image>` |
| i18n | react-i18next |
| Unit / logic tests | Vitest |
| Component tests | React Testing Library + Vitest |
| E2E tests | Playwright |

## Development Workflow

- **Branch strategy**: Feature branches off `main`; naming convention `###-kebab-feature-name`.
- **Spec-first**: Every non-trivial feature MUST have a spec (`/speckit.specify`) before any code
  is written. Plans (`/speckit.plan`) MUST include a Constitution Check gate.
- **Module addition**: New modules MUST be proposed in a plan and reviewed before scaffolding.
  New modules MUST register via `Add<Module>Module` and `Use<Module>Module` extension methods.
- **Database migrations**: Migrations MUST be generated per module and MUST NOT cross module
  boundaries. Migration file names MUST include the feature branch name.
- **Docker Compose**: All infrastructure dependencies (PostgreSQL, Redis, Keycloak, Seq,
  RabbitMQ, MinIO) MUST be available via `docker-compose up` before running any integration test.
- **CI gate**: The pipeline MUST run all of the following before any merge to `main`:
  - `dotnet test` — all backend integration + unit tests
  - `bun run build` — both frontend projects (Portal + Order)
  - `bun run test` — Vitest unit/component suite for both frontends
  - Lighthouse CI — `MenuSnap.Web.Order` customer-facing routes MUST score ≥ 90 on mobile
    Performance and LCP < 2.5 s; failures block the merge.

## Governance

This constitution supersedes all other architectural guidelines, README conventions, and informal
agreements within the MenuSnap.Server, MenuSnap.Web.Portal, and MenuSnap.Web.Order repositories.

**Amendment procedure**:
1. Propose amendment in a spec or ADR (Architecture Decision Record) linked to the feature branch.
2. The amendment MUST be applied via `/speckit.constitution` with a rationale in the user input.
3. Any principle change MUST include a migration plan for existing code that violates the new rule.

**Versioning policy** (SemVer):
- MAJOR: Removal or redefinition of an existing principle.
- MINOR: Addition of a new principle or substantial expansion of an existing one.
- PATCH: Wording clarification, formatting, or non-semantic correction.

**Compliance review**: Every plan (`plan.md`) MUST include a *Constitution Check* section that
explicitly verifies compliance with all seven principles before Phase 0 research begins.

Complexity violations (deliberate deviations) MUST be documented in the plan's *Complexity
Tracking* table with justification and rejected alternatives.

**Version**: 1.2.1 | **Ratified**: 2026-03-15 | **Last Amended**: 2026-03-15
