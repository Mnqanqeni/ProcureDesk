namespace ProcureDesk.Domain;

public interface ISupplierRepository
{
    IEnumerable<Supplier> List();
    Supplier? FindByCode(string code);
    void Add(Supplier supplier);
    void Update(Supplier supplier);
    void Delete(string code);
}
