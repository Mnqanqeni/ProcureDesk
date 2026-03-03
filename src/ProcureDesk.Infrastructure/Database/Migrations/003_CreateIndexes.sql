USE ProcureDesk;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PO_Supplier')
BEGIN
    CREATE INDEX IX_PO_Supplier
        ON dbo.PurchaseOrder (SupplierCode);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_POLine_PO')
BEGIN
    CREATE INDEX IX_POLine_PO
        ON dbo.PurchaseOrderLine (PurchaseOrderId);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_POLine_Product')
BEGIN
    CREATE INDEX IX_POLine_Product
        ON dbo.PurchaseOrderLine (ProductCode);
END
GO