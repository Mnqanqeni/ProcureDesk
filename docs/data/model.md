# ProcureDesk — Domain Model

This document describes the core domain objects and relationships.

---

## Entities

### Good
A master record representing an item the organization can purchase.

**Key fields**
- `Code` (string, unique, stable identifier)
- `Name` (string)
- `IsActive` (bool)

**Notes**
- Pricing and supplier-specific naming do not belong on Good.

---

### Supplier
A vendor the organization purchases goods from.

**Key fields**
- `Id` (guid/int)
- `Name` (string)
- `IsActive` (bool)

---

### SupplierGood (Link)
Represents the relationship “Supplier sells Good” with supplier-specific details.

**Key fields**
- `SupplierId`
- `GoodCode`
- `SupplierSku` (string)
- `UnitPrice` (decimal)
- `LeadTimeDays` (int)
- `IsPreferred` (bool, optional)

**Notes**
- This is where “different names and prices per supplier” lives.

---

## Purchase Order Aggregate

### PurchaseOrder (Aggregate Root)
Represents an order placed to one supplier.

**Key fields**
- `Id`
- `OrderNumber` (optional)
- `SupplierId`
- `RequiredDeliveryDate`
- `Status` (Draft, Submitted, Cancelled, Received)

**Core rules**
- One purchase order belongs to exactly one supplier
- After submission, editing may be restricted (recommended)

---

### PurchaseOrderLine
A line item inside a PurchaseOrder.

**Key fields**
- `PurchaseOrderId`
- `GoodCode`
- `Quantity`
- `UnitPrice` (captured at the time of ordering)

**Core rules**
- Quantity must be > 0
- Good must be linked to the supplier of the purchase order

---

## Relationships (High Level)

- Supplier 1..* ↔ *..1 Good (through SupplierGood)
- PurchaseOrder 1..* → PurchaseOrderLine
- PurchaseOrderLine *..1 → Good (by GoodCode)
- PurchaseOrder *..1 → Supplier

---

## Status definitions (suggested)

- **Draft**: editable
- **Submitted**: sent/confirmed, limited edits
- **Received**: delivered/complete
- **Cancelled**: closed without delivery
