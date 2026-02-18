namespace ProcureDesk.Domain;

public class PurchaseOrderLine
{
    public Guid Id{ get; init; }
    public Product Good { get; init; }
    public int Quantity { get;}
    public decimal Price;

    public PurchaseOrderLine( Product good, int quantity, decimal unitPrice)
    {
        Good = good;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static (bool isValid, List<string> errors) Validate(Product good, int quantity, decimal unitPrice)
    {
        var errors = new List<string>();

        if (good == null)
            errors.Add("Good is required.");

        if (quantity <= 0)
            errors.Add("Quantity must be greater than 0.");

        if (unitPrice < 0)
            errors.Add("Unit price cannot be negative.");

        return errors.Count == 0
            ? (true, errors)
            : (false, errors);
    }
}
