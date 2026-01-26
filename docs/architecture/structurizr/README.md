# ProcureDesk

ProcureDesk is a Clean Architecture ASP.NET Core Purchase Order System built for practice and portfolio use.

It models a realistic procurement domain where goods are purchased from multiple suppliers, each with different item names, pricing, and lead times.

## Features

- Maintain Goods (create, update, deactivate)
- Maintain Suppliers (create, update, deactivate)
- Link Goods to Suppliers (supplier SKU, unit price, lead time, preferred supplier)
- Manage Purchase Orders (draft, add/remove lines, submit)
- Validate ordering rules (only goods linked to the selected supplier)
- Cancel overdue purchase orders

## Architecture

This project follows Onion / Clean Architecture:

- **Domain**: business entities and rules
- **Application**: use-cases and interfaces (ports)
- **Infrastructure**: EF Core, repositories, persistence
- **API**: HTTP controllers and DTOs

A C4 model is available in:  
`docs/architecture/structurizr/workspace.dsl`

## Repository Structure

```text
src/
  ProcureDesk.Api/
  ProcureDesk.Application/
  ProcureDesk.Domain/
  ProcureDesk.Infrastructure/

docs/
  architecture/
  requirements/
  data/
  decisions/

frontend/ (optional, added later)
