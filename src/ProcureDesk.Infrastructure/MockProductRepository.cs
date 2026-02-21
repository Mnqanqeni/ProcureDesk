using ProcureDesk.Domain;
using System.Collections.Generic;

public class MockProductRepository : IProductRepository
{
    private readonly Dictionary<string, Product> _products = new();

    public IEnumerable<Product> GetAll() => _products.Values;

    public Product? Get(string code)
        => _products.TryGetValue(code, out var product) ? product : null;

    public void Add(Product product)
    {
        _products[product.Code] = product;
    }

    public void Update(Product product)
    {
        _products[product.Code] = product;
    }

    public void Delete(string code)
    {
        _products.Remove(code);
    }
}
