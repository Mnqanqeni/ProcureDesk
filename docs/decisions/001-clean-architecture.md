# ADR 001 — Use Clean / Onion Architecture

## Context
The system includes:
- domain rules (procurement concepts)
- use-cases (workflows)
- persistence (database)
- HTTP API

Mixing these concerns in one project makes testing and changes difficult.

## Decision
Use a layered architecture:

- **Domain**: entities, value objects, invariants
- **Application**: use-cases and interfaces (ports)
- **Infrastructure**: persistence and external integrations (adapters)
- **API**: controllers, DTOs, and wiring

Dependencies flow inward:
- API → Application → Domain
- Infrastructure → Application + Domain (implements Application interfaces)

## Consequences
- Domain remains independent of EF Core and ASP.NET Core
- Application can be tested with in-memory repositories
- Infrastructure can be replaced (change DB/provider) with minimal impact
