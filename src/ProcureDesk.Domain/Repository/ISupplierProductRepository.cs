namespace ProcureDesk.Domain;

public interface ISupplierProductRepository
{
    SupplierProduct? Get(string supplierCode, string productCode);
    List<SupplierProduct> GetBySupplier(string supplierCode);
    List<SupplierProduct> GetByProduct(string productCode);

    void Add(SupplierProduct link);
    void Update(SupplierProduct link);
    void Delete(string supplierCode, string productCode);
}
