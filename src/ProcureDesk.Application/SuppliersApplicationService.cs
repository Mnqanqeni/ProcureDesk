using ProcureDesk.Domain;

namespace ProcureDesk.Application;

public class SuppliersApplicationService
{
    private readonly ISupplierRepository _repo;

    public SuppliersApplicationService(ISupplierRepository repo)
    {
        _repo = repo;
    }

    public (bool IsSuccess, IEnumerable<Supplier>? Value, string Error) ListSuppliers()
        => (true, _repo.List(), string.Empty);

    public (bool IsSuccess, Supplier? Value, string Error) GetSupplierByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return (false, default(Supplier), "Code is required.");

        var trimmed = code.Trim();

        var supplier = _repo.FindByCode(trimmed);
        return supplier is null
            ? (false, default(Supplier), "Supplier not found.")
            : (true, supplier, string.Empty);
    }

    public (bool IsSuccess, string Error) CreateSupplier(string code, string name)
    {
        var (isValid, message) = Supplier.Validate(code, name);
        if (!isValid)
            return (false, message);

        var trimmedCode = code.Trim();
        var trimmedName = name.Trim();

        if (_repo.FindByCode(trimmedCode) is not null)
            return (false, "A supplier with this code already exists.");

        _repo.Add(new Supplier(trimmedCode, trimmedName));
        return (true, string.Empty);
    }

    public (bool IsSuccess, string Error) RenameSupplier(string code, string newName)
    {
        if (string.IsNullOrWhiteSpace(code))
            return (false, "Code is required.");

        // Reuse Supplier.Validate: same required rules
        var (isValid, message) = Supplier.Validate(code, newName);
        if (!isValid)
            return (false, message);

        var trimmedCode = code.Trim();

        var supplier = _repo.FindByCode(trimmedCode);
        if (supplier is null)
            return (false, "Supplier not found.");

        supplier.Name = newName.Trim();
        _repo.Update(supplier);

        return (true, string.Empty);
    }

    public (bool IsSuccess, string Error) DeleteSupplier(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return (false, "Code is required.");

        var trimmedCode = code.Trim();

        var supplier = _repo.FindByCode(trimmedCode);
        if (supplier is null)
            return (false, "Supplier not found.");

        _repo.Delete(trimmedCode);
        return (true, string.Empty);
    }
}
