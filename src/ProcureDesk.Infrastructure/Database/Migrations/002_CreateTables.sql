USE ProcureDesk;
GO

-- =========================
-- Supplier
-- =========================
IF OBJECT_ID('dbo.Supplier', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Supplier (
        Code        VARCHAR(50)  NOT NULL PRIMARY KEY,
        Name        VARCHAR(200) NOT NULL,
        LeadTimeDays INT         NULL,

        CreatedDate DATETIME2 NOT NULL,
        EditDate    DATETIME2 NOT NULL,
        CreateUser  VARCHAR(100) NOT NULL,
        EditUser    VARCHAR(100) NOT NULL
    );
END
GO

-- =========================
-- Product
-- =========================
IF OBJECT_ID('dbo.Product', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Product (
        Code        VARCHAR(50)  NOT NULL PRIMARY KEY,
        Name        VARCHAR(200) NOT NULL,

        CreatedDate DATETIME2 NOT NULL,
        EditDate    DATETIME2 NOT NULL,
        CreateUser  VARCHAR(100) NOT NULL,
        EditUser    VARCHAR(100) NOT NULL
    );
END
GO

-- =========================
-- SupplierProduct
-- =========================
IF OBJECT_ID('dbo.SupplierProduct', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SupplierProduct (
        SupplierCode VARCHAR(50) NOT NULL,
        ProductCode  VARCHAR(50) NOT NULL,
        SupplierProductName VARCHAR(200) NOT NULL,
        Price DECIMAL(18,2) NULL,
        LeadTimeDays INT NULL,

        CreatedDate DATETIME2 NOT NULL,
        EditDate    DATETIME2 NOT NULL,
        CreateUser  VARCHAR(100) NOT NULL,
        EditUser    VARCHAR(100) NOT NULL,

        CONSTRAINT PK_SupplierProduct PRIMARY KEY (SupplierCode, ProductCode),
        CONSTRAINT FK_SupplierProduct_Supplier FOREIGN KEY (SupplierCode)
            REFERENCES dbo.Supplier(Code),
        CONSTRAINT FK_SupplierProduct_Product FOREIGN KEY (ProductCode)
            REFERENCES dbo.Product(Code)
    );
END
GO

-- =========================
-- PurchaseOrder
-- =========================
IF OBJECT_ID('dbo.PurchaseOrder', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PurchaseOrder (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        OrderNumber VARCHAR(50) NOT NULL UNIQUE,
        SupplierCode VARCHAR(50) NOT NULL,
        Status INT NOT NULL,

        CreatedDate DATETIME2 NOT NULL,
        EditDate    DATETIME2 NOT NULL,
        CreateUser  VARCHAR(100) NOT NULL,
        EditUser    VARCHAR(100) NOT NULL,

        CONSTRAINT FK_PurchaseOrder_Supplier
            FOREIGN KEY (SupplierCode) REFERENCES dbo.Supplier(Code)
    );
END
GO

-- =========================
-- PurchaseOrderLine
-- =========================
IF OBJECT_ID('dbo.PurchaseOrderLine', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PurchaseOrderLine (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        PurchaseOrderId UNIQUEIDENTIFIER NOT NULL,
        ProductCode VARCHAR(50) NOT NULL,
        Quantity INT NOT NULL,
        Price DECIMAL(18,2) NOT NULL,

        CreatedDate DATETIME2 NOT NULL,
        EditDate    DATETIME2 NOT NULL,
        CreateUser  VARCHAR(100) NOT NULL,
        EditUser    VARCHAR(100) NOT NULL,

        CONSTRAINT FK_POLine_PO
            FOREIGN KEY (PurchaseOrderId) REFERENCES dbo.PurchaseOrder(Id),

        CONSTRAINT FK_POLine_Product
            FOREIGN KEY (ProductCode) REFERENCES dbo.Product(Code)
    );
END
GO