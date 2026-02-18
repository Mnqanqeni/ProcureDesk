namespace ProcureDesk.Domain;

public class PurchaseOrder
{
    public required Guid Id { get; init; }
    public required string OrderNumber { get; init; }
    public required string SupplierCode { get; init; }

    public required PurchaseOrderStatus Status { get; set; }
    public required List<PurchaseOrderLine> Lines { get; init; }

    public required DateTime CreatedDate { get; set; }
    public required DateTime EditDate { get; set; }
    public required string CreateUser { get; set; }
    public required string EditUser { get; set; }

    public decimal Total => Lines.Sum(l => l.Quantity * l.Price);

    private PurchaseOrder() { }

    // ---------------- CREATE ----------------

    public static (bool isValid, List<string> errors, PurchaseOrder? order)
        Create(
            string orderNumber,
            string supplierCode,
            Product product,
            int quantity,
            decimal price,
            string user)
    {
        var errors = new List<string>();

        var (headerOk, headerErrors) = Validate(orderNumber, supplierCode);
        if (!headerOk) errors.AddRange(headerErrors);

        var (lineOk, lineErrors, line) = PurchaseOrderLine.Create(product, quantity, price, user);
        if (!lineOk) errors.AddRange(lineErrors);

        if (errors.Count > 0)
            return (false, errors, null);

        var now = DateTime.UtcNow;

        return (true, errors, new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = orderNumber,
            SupplierCode = supplierCode,
            Status = PurchaseOrderStatus.Draft,
            Lines = new List<PurchaseOrderLine> { line! },
            CreatedDate = now,
            EditDate = now,
            CreateUser = user,
            EditUser = user
        });
    }

    // ---------------- VALIDATE ----------------

    public static (bool isValid, List<string> errors)
        Validate(string orderNumber, string supplierCode)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(orderNumber))
            errors.Add("Order number is required.");

        if (string.IsNullOrWhiteSpace(supplierCode))
            errors.Add("Supplier code is required.");

        return (errors.Count == 0, errors);
    }

    // ---------------- ADD LINE ----------------

    public (bool ok, List<string> errors)
        AddLine(Product product, int quantity, decimal price, string user)
    {
        var errors = new List<string>();

        if (Status != PurchaseOrderStatus.Draft)
            errors.Add("Lines can only be added when order is Draft.");

        var (lineOk, lineErrors, line) = PurchaseOrderLine.Create(product, quantity, price, user);
        if (!lineOk) errors.AddRange(lineErrors);

        if (errors.Count > 0)
            return (false, errors);

        Lines.Add(line!);
        Touch(user);

        return (true, errors);
    }

    // ---------------- REMOVE LINE ----------------

    public (bool ok, List<string> errors)
        RemoveLine(Guid lineId, string user)
    {
        var errors = new List<string>();

        if (Status != PurchaseOrderStatus.Draft)
            errors.Add("Lines can only be removed when order is Draft.");

        var line = Lines.FirstOrDefault(l => l.Id == lineId);
        if (line == null)
            errors.Add("Line not found.");

        if (Lines.Count == 1)
            errors.Add("Purchase order must have at least one line.");

        if (errors.Count > 0)
            return (false, errors);

        Lines.Remove(line!);
        Touch(user);

        return (true, errors);
    }

    // ---------------- UPDATE QUANTITY ----------------

    public (bool ok, List<string> errors)
        UpdateLineQuantity(Guid lineId, int quantity, string user)
    {
        var errors = new List<string>();

        if (Status != PurchaseOrderStatus.Draft)
            errors.Add("Lines can only be edited when order is Draft.");

        var line = Lines.FirstOrDefault(l => l.Id == lineId);
        if (line == null)
            errors.Add("Line not found.");

        if (quantity <= 0)
            errors.Add("Quantity must be greater than 0.");

        if (errors.Count > 0)
            return (false, errors);

        line!.Quantity = quantity;
        Touch(user);

        return (true, errors);
    }

    // ---------------- UPDATE PRICE ----------------

    public (bool ok, List<string> errors)
        UpdateLinePrice(Guid lineId, decimal price, string user)
    {
        var errors = new List<string>();

        if (Status != PurchaseOrderStatus.Draft)
            errors.Add("Lines can only be edited when order is Draft.");

        var line = Lines.FirstOrDefault(l => l.Id == lineId);
        if (line == null)
            errors.Add("Line not found.");

        if (price < 0)
            errors.Add("Price cannot be negative.");

        if (errors.Count > 0)
            return (false, errors);

        line!.Price = price;
        Touch(user);

        return (true, errors);
    }

    // ---------------- SUBMIT ----------------

    public (bool ok, List<string> errors)
        Submit(string user)
    {
        var errors = new List<string>();

        if (Status != PurchaseOrderStatus.Draft)
            errors.Add("Only Draft orders can be submitted.");

        if (Lines.Count == 0)
            errors.Add("Purchase order must have at least one line.");

        if (errors.Count > 0)
            return (false, errors);

        Status = PurchaseOrderStatus.Submitted;
        Touch(user);

        return (true, errors);
    }

    // ---------------- CANCEL ----------------

    public (bool ok, List<string> errors)
        Cancel(string user)
    {
        var errors = new List<string>();

        if (Status == PurchaseOrderStatus.Delivered)
            errors.Add("Delivered orders cannot be cancelled.");

        if (errors.Count > 0)
            return (false, errors);

        Status = PurchaseOrderStatus.Cancelled;
        Touch(user);

        return (true, errors);
    }

    // ---------------- HELPER ----------------

    private void Touch(string user)
    {
        EditUser = user;
        EditDate = DateTime.UtcNow;
    }
}
