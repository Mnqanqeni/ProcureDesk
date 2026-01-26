workspace "ProcureDesk" "C4 model for a procurement Purchase Order system" {

  !identifiers hierarchical

  model {
    user = person "Procurement User" "Maintains goods and suppliers, links supplier catalog items, and creates purchase orders."

    procureDesk = softwareSystem "ProcureDesk" "Internal procurement system for managing goods, suppliers, and purchase orders." {

      web = container "Web Application" "User interface for procurement operations." "Web App"

      api = container "Backend API" "REST API implementing procurement workflows." "ASP.NET Core Web API" {

        // Controllers (HTTP layer)
        goodsController = component "Goods Controller" "HTTP endpoints for goods." "ASP.NET Core Controller"
        suppliersController = component "Suppliers Controller" "HTTP endpoints for suppliers." "ASP.NET Core Controller"
        supplierGoodsController = component "Supplier-Goods Controller" "HTTP endpoints for linking goods to suppliers (price, SKU, lead time)." "ASP.NET Core Controller"
        purchaseOrdersController = component "Purchase Orders Controller" "HTTP endpoints for purchase orders and line items." "ASP.NET Core Controller"

        // Application services (use-cases)
        goodsService = component "Goods Application Service" "Use-cases for goods." "C# Service"
        suppliersService = component "Suppliers Application Service" "Use-cases for suppliers." "C# Service"
        supplierGoodsService = component "Supplier-Goods Application Service" "Use-cases for linking suppliers and goods." "C# Service"
        purchaseOrdersService = component "Purchase Orders Application Service" "Use-cases for purchase orders (draft, add lines, submit, cancel overdue)." "C# Service"

        // Repositories (data access)
        goodsRepository = component "Goods Repository" "Persists and queries goods." "EF Core Repository"
        suppliersRepository = component "Suppliers Repository" "Persists and queries suppliers." "EF Core Repository"
        supplierGoodsRepository = component "Supplier-Goods Repository" "Persists and queries supplier-good links." "EF Core Repository"
        purchaseOrdersRepository = component "Purchase Orders Repository" "Persists and queries purchase orders and lines." "EF Core Repository"

        // Domain
        domainModel = component "Domain Model" "Entities and rules: Good, Supplier, SupplierGood, PurchaseOrder, PurchaseOrderLine." "C# Domain"
      }

      db = container "Database" "Stores master data and purchase orders." "SQL Server / PostgreSQL" {
        tags "Database"
      }
    }

    // System/Container relationships
    user -> procureDesk.web "Uses" "HTTPS"
    procureDesk.web -> procureDesk.api "Calls REST API" "HTTPS/JSON"
    procureDesk.api -> procureDesk.db "Reads/Writes" "ADO.NET"

    // Component relationships (inside Backend API) - use full identifiers
    procureDesk.api.goodsController -> procureDesk.api.goodsService "Calls"
    procureDesk.api.suppliersController -> procureDesk.api.suppliersService "Calls"
    procureDesk.api.supplierGoodsController -> procureDesk.api.supplierGoodsService "Calls"
    procureDesk.api.purchaseOrdersController -> procureDesk.api.purchaseOrdersService "Calls"

    procureDesk.api.goodsService -> procureDesk.api.domainModel "Creates/validates"
    procureDesk.api.suppliersService -> procureDesk.api.domainModel "Creates/validates"
    procureDesk.api.supplierGoodsService -> procureDesk.api.domainModel "Creates/validates"
    procureDesk.api.purchaseOrdersService -> procureDesk.api.domainModel "Creates/validates"

    procureDesk.api.goodsService -> procureDesk.api.goodsRepository "Loads/saves"
    procureDesk.api.suppliersService -> procureDesk.api.suppliersRepository "Loads/saves"
    procureDesk.api.supplierGoodsService -> procureDesk.api.supplierGoodsRepository "Loads/saves"
    procureDesk.api.purchaseOrdersService -> procureDesk.api.purchaseOrdersRepository "Loads/saves"

    procureDesk.api.goodsRepository -> procureDesk.db "Reads/Writes"
    procureDesk.api.suppliersRepository -> procureDesk.db "Reads/Writes"
    procureDesk.api.supplierGoodsRepository -> procureDesk.db "Reads/Writes"
    procureDesk.api.purchaseOrdersRepository -> procureDesk.db "Reads/Writes"
  }

  views {
    systemContext procureDesk "SystemContext" {
      include *
      autolayout lr
      title "System Context - ProcureDesk"
    }

    container procureDesk "Containers" {
      include *
      autolayout lr
      title "Container View - ProcureDesk"
    }

    component procureDesk.api "BackendComponents" {
      include *
      autolayout lr
      title "Component View - Backend API"
    }

    styles {
      element "Person" {
        shape person
      }

      element "Database" {
        shape cylinder
      }

      element "Software System" {
        background "#0b3d91"
        color "#ffffff"
      }

      element "Container" {
        background "#1168bd"
        color "#ffffff"
      }

      element "Component" {
        background "#2d8bd3"
        color "#ffffff"
      }
    }
  }
}
