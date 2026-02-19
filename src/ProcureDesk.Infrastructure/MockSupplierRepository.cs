using ProcureDesk.Domain;
using System.Collections.Generic;

public class MockSupplierRepository : ISupplierRepository
{
    private readonly Dictionary<string, Supplier> _suppliers = new();

    public IEnumerable<Supplier> List() => _suppliers.Values;

    public Supplier? GetByCode(string code)
        => _suppliers.TryGetValue(code, out var supplier) ? supplier : null;

    public void Add(Supplier supplier)
    {
        if (_suppliers.ContainsKey(supplier.Code))
            throw new InvalidOperationException($"Supplier with code '{supplier.Code}' already exists.");

        _suppliers[supplier.Code] = supplier;
    }

    public void Update(Supplier supplier)
    {
        if (!_suppliers.ContainsKey(supplier.Code))
            throw new KeyNotFoundException($"Supplier with code '{supplier.Code}' not found.");

        _suppliers[supplier.Code] = supplier;
    }

    public void Delete(string code)
    {
        _suppliers.Remove(code);
    }
}
