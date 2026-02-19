namespace ProcureDesk.Application;

using ProcureDesk.Domain;

public class SuppliersApplicationService
{
    private readonly ISupplierRepository _suppliers;

    public SuppliersApplicationService(ISupplierRepository suppliers)
    {
        _suppliers = suppliers;
    }

    public IEnumerable<Supplier> List() => _suppliers.List();

    public Supplier? GetByCode(string code) => _suppliers.GetByCode(code);

    public (bool ok, List<string> errors, Supplier? supplier)
        Create(string code, string name, string user)
    {

        var (isValid, errors, supplier) = Supplier.Create(code, name, user);
        if (!isValid) return (false, errors, null);

        if (_suppliers.GetByCode(code) != null)
            return (false, new List<string> { "Supplier code already exists." }, null);

        _suppliers.Add(supplier!);

        return (true, new List<string>(), supplier);
    }

    public (bool ok, List<string> errors)
        UpdateName(string code, string newName, string user)
    {
        var supplier = _suppliers.GetByCode(code);
        if (supplier is null)
            return (false, new List<string> { "Supplier not found." });

        var (isValid, errors) = Supplier.Validate(supplier.Code, newName);
        if (!isValid) return (false, errors);

        supplier.Name = newName;
        supplier.EditUser = user;
        supplier.EditDate = DateTime.UtcNow;

        _suppliers.Update(supplier);

        return (true, new List<string>());
    }

    public (bool ok, List<string> errors)
        Delete(string code)
    {
        if (_suppliers.GetByCode(code) is null)
            return (false, new List<string> { "Supplier not found." });

        _suppliers.Delete(code);
        return (true, new List<string>());
    }
}
