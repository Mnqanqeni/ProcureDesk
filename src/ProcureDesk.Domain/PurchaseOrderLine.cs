namespace ProcureDesk.Domain;

public class PurchaseOrderLine
{
    public int LineNumber;
    public string GoodCode;
    public int Quantity;
    public decimal UnitPrice;

    public PurchaseOrderLine(int lineNumber, string goodCode, int quantity, decimal unitPrice)
    {
        LineNumber = lineNumber;
        GoodCode = goodCode;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static (bool isValid, string message) Validate(string goodCode, int quantity, decimal unitPrice)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(goodCode))
            errors.Add("Good code is required.");

        if (quantity <= 0)
            errors.Add("Quantity must be greater than 0.");

        if (unitPrice < 0)
            errors.Add("Unit price cannot be negative.");

        return errors.Count == 0
            ? (true, string.Empty)
            : (false, string.Join(" ", errors));
    }
}
