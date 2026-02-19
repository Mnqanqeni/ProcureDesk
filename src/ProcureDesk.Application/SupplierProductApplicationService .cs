namespace ProcureDesk.Application;

using ProcureDesk.Domain;

public class SupplierProductApplicationService
{
    private readonly ISupplierProductRepository _repo;

    public SupplierProductApplicationService(ISupplierProductRepository repo)
    {
        _repo = repo;
    }

    public (bool isValid, List<string> errors) Create(
        string supplierCode,
        string productCode,
        string supplierProductName,
        string user)
    {
        var (isValid, errors, link) =
            SupplierProduct.Create(supplierCode, productCode, supplierProductName, user);

        if (!isValid || link == null)
            return (false, errors);

        var existing = _repo.Get(supplierCode, productCode);
        if (existing != null)
            return (false, ["Supplier already supplies this product."]);

        _repo.Add(link);

        return (true, []);
    }

    public void UpdatePrice(
        string supplierCode,
        string productCode,
        decimal? price,
        string user)
    {
        var link = _repo.Get(supplierCode, productCode);
        if (link == null) return;

        link.Price = price;
        link.EditDate = DateTime.UtcNow;
        link.EditUser = user;

        _repo.Update(link);
    }

    public void UpdateLeadTime(
        string supplierCode,
        string productCode,
        int? leadTimeDays,
        string user)
    {
        var link = _repo.Get(supplierCode, productCode);
        if (link == null) return;

        link.LeadTimeDays = leadTimeDays;
        link.EditDate = DateTime.UtcNow;
        link.EditUser = user;

        _repo.Update(link);
    }
}
