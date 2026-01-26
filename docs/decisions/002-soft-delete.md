# ADR 002 — Use IsActive instead of hard delete

## Context
Goods and suppliers may be referenced by historical purchase orders.
Hard deleting them risks breaking history and reports.

## Decision
Use a boolean flag:
- `IsActive = true` means selectable for new work
- `IsActive = false` means retired/deactivated

Deactivated items are excluded from selection lists for new purchase orders, but remain available for historical records.

## Consequences
- Historical purchase orders remain valid and readable
- Prevents orphaned references
- Requires filtering (`IsActive = true`) in queries that build selection lists
