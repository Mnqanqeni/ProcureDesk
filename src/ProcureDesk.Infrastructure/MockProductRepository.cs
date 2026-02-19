using ProcureDesk.Domain;
using System.Collections.Generic;

public class MockProductRepository : IProductRepository
{
    private readonly Dictionary<string, Product> _products = new();

    public IEnumerable<Product> List() => _products.Values;

    public Product? GetByCode(string code)
        => _products.TryGetValue(code, out var product) ? product : null;

    public void Add(Product product)
    {
        if (_products.ContainsKey(product.Code))
            throw new InvalidOperationException($"Product with code '{product.Code}' already exists.");

        _products[product.Code] = product;
    }

    public void Update(Product product)
    {
        if (!_products.ContainsKey(product.Code))
            throw new KeyNotFoundException($"Product with code '{product.Code}' not found.");

        _products[product.Code] = product;
    }

    public void Delete(string code)
    {
        _products.Remove(code);
    }
}
