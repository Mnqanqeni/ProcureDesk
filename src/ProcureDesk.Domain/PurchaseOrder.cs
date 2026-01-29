namespace ProcureDesk.Domain;

public class PurchaseOrder
{
    public string OrderNumber;
    public string SupplierCode;
    public PurchaseOrderStatus Status;
    public List<PurchaseOrderLine> Lines;

    public PurchaseOrder(string orderNumber, string supplierCode)
    {
        OrderNumber = orderNumber;
        SupplierCode = supplierCode;
        Status = PurchaseOrderStatus.Draft;
        Lines = new List<PurchaseOrderLine>();
    }

    public static (bool isValid, string message) Validate(string orderNumber, string supplierCode)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(orderNumber))
            errors.Add("Order number is required.");

        if (string.IsNullOrWhiteSpace(supplierCode))
            errors.Add("Supplier code is required.");

        return errors.Count == 0
            ? (true, string.Empty)
            : (false, string.Join(" ", errors));
    }
}
