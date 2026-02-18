namespace ProcureDesk.Domain;

public interface IProductRepository
{
    IEnumerable<Product> List();
    Product? GetByCode(string code);
    void Add(Product product);
    void Update(Product product);
    void Delete(string code);
}
