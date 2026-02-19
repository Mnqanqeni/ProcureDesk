using ProcureDesk.Domain;

namespace ProcureDesk.Infrastructure;

public class MockSupplierProductRepository : ISupplierProductRepository
{
    private readonly List<SupplierProduct> _data = [];

    public SupplierProduct? Get(string supplierCode, string productCode)
        => _data.FirstOrDefault(x =>
            x.SupplierCode == supplierCode &&
            x.ProductCode == productCode);

    public List<SupplierProduct> GetBySupplier(string supplierCode)
        => _data.Where(x => x.SupplierCode == supplierCode).ToList();

    public List<SupplierProduct> GetByProduct(string productCode)
        => _data.Where(x => x.ProductCode == productCode).ToList();

    public void Add(SupplierProduct link)
        => _data.Add(link);

    public void Update(SupplierProduct link)
    {
        var existing = Get(link.SupplierCode, link.ProductCode);
        if (existing == null) return;

        existing.SupplierProductName = link.SupplierProductName;
        existing.Price = link.Price;
        existing.LeadTimeDays = link.LeadTimeDays;
        existing.EditDate = link.EditDate;
        existing.EditUser = link.EditUser;
    }

    public void Delete(string supplierCode, string productCode)
        => _data.RemoveAll(x =>
            x.SupplierCode == supplierCode &&
            x.ProductCode == productCode);
}
