namespace ProcureDesk.Domain;

public class PurchaseOrderLine
{
    public required Guid Id { get; init; }
    public required Product Product { get; init; }

    public required int Quantity { get; set; }
    public required decimal Price { get; set; }

    public decimal Amount => Quantity * Price;

    public required DateTime CreatedDate { get; set; }
    public required DateTime EditDate { get; set; }
    public required string CreateUser { get; set; }
    public required string EditUser { get; set; }

    private PurchaseOrderLine() { }

    public static (bool isValid, List<string> errors, PurchaseOrderLine? line)
        Create(Product? product, int quantity, decimal price, string user)
    {
        var (isValid, errors) = Validate(product, quantity, price);
        if (!isValid) return (false, errors, null);

        var now = DateTime.UtcNow;

        return (true, errors, new PurchaseOrderLine
        {
            Id = Guid.NewGuid(),
            Product = product!,  
            Quantity = quantity,
            Price = price,
            CreatedDate = now,
            EditDate = now,
            CreateUser = user,
            EditUser = user
        });
    }

    public static (bool isValid, List<string> errors)
        Validate(Product? product, int quantity, decimal price)
    {
        var errors = new List<string>();

        if (product is null)
            errors.Add("Product is required.");

        if (quantity <= 0)
            errors.Add("Quantity must be greater than 0.");

        if (price < 0)
            errors.Add("Price cannot be negative.");

        return (errors.Count == 0, errors);
    }
}
