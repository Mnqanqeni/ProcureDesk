namespace ProcureDesk.Application;

using ProcureDesk.Domain;

public class ProductApplicationService
{
    private readonly IProductRepository _products;

    public ProductApplicationService(IProductRepository products)
    {
        _products = products;
    }

    public IEnumerable<Product> List() => _products.List();

    public Product? GetByCode(string code) => _products.GetByCode(code);

    public (bool ok, List<string> errors, Product? product)
        Create(string code, string name, string user)
    {
     
        var (isValid, errors, product) = Product.Create(code, name, user);
        if (!isValid) return (false, errors, null);

        if (_products.GetByCode(code) != null)
            return (false, new List<string> { "Product code already exists." }, null);

        _products.Add(product!);

        return (true, new List<string>(), product);
    }

    public (bool ok, List<string> errors)
        UpdateName(string code, string newName, string user)
    {
        var product = _products.GetByCode(code);
        if (product is null)
            return (false, new List<string> { "Product not found." });

        var (isValid, errors) = Product.Validate(product.Code, newName);
        if (!isValid) return (false, errors);

        product.Name = newName;
        product.EditUser = user;
        product.EditDate = DateTime.UtcNow;

        _products.Update(product);

        return (true, new List<string>());
    }

    public (bool ok, List<string> errors)
        Delete(string code)
    {
        if (_products.GetByCode(code) is null)
            return (false, new List<string> { "Product not found." });

        _products.Delete(code);
        return (true, new List<string>());
    }
}
