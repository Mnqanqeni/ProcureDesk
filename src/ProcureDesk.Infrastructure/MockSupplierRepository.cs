using ProcureDesk.Domain;

namespace ProcureDesk.Infrastructure;

public class MockSupplierRepository : ISupplierRepository
{
    private readonly List<Supplier> _suppliers = new();

    public IEnumerable<Supplier> List()
        => _suppliers.ToList();

    public Supplier? FindByCode(string code)
        => _suppliers.FirstOrDefault(s => s.Code == code);

    public void Add(Supplier supplier)
        => _suppliers.Add(supplier);

    public void Update(Supplier supplier)
    {
        var existing = FindByCode(supplier.Code);
        if (existing is not null)
        {
            _suppliers.Remove(existing);
            _suppliers.Add(supplier);
        }
    }

    public void Delete(string code)
    {
        var existing = FindByCode(code);
        if (existing is not null)
            _suppliers.Remove(existing);
    }
}
