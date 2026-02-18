namespace ProcureDesk.Domain;

public class SupplierProduct
{
    public required string SupplierCode { get; init; }
    public required string ProductCode { get; init; }

    public required string SupplierProductName { get; set; }
    public decimal? Price { get; set; }
    public int? LeadTimeDays { get; set; }

    public required DateTime CreatedDate { get; set; }
    public required DateTime EditDate { get; set; }
    public required string CreateUser { get; set; }
    public required string EditUser { get; set; }

    private SupplierProduct() { }

    public static (bool isValid, List<string> errors, SupplierProduct? link)
        Create(string supplierCode, string productCode, string supplierProductName, string user)
    {
        var (isValid, errors) = Validate(supplierCode, productCode, supplierProductName);
        if (!isValid) return (false, errors, null);

        var now = DateTime.UtcNow;

        return (true, errors, new SupplierProduct
        {
            SupplierCode = supplierCode,
            ProductCode = productCode,
            SupplierProductName = supplierProductName,
            CreatedDate = now,
            EditDate = now,
            CreateUser = user,
            EditUser = user
        });
    }

    public static (bool isValid, List<string> errors)
        Validate(string supplierCode, string productCode, string supplierProductName)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(supplierCode))
            errors.Add("Supplier code is required.");

        if (string.IsNullOrWhiteSpace(productCode))
            errors.Add("Product code is required.");

        if (string.IsNullOrWhiteSpace(supplierProductName))
            errors.Add("Supplier product name is required.");

        return (errors.Count == 0, errors);
    }
}
