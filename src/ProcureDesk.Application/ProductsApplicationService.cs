namespace ProcureDesk.Application;

using ProcureDesk.Domain;

public class ProductApplicationService
{
    private readonly IProductRepository _productRepository;

    public ProductApplicationService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IEnumerable<Product> GetAll() => _productRepository.GetAll();

    public Product? Get(string code) => _productRepository.Get(code);

    public (bool ok, List<string> errors, Product? product)
        Create(string code, string name, string user)
    {
        var (isValid, errors, product) = Product.Create(code, name, user);
        if (!isValid) return (false, errors, null);

        
        if (_productRepository.Get(product!.Code) is not null)
            return (false, new List<string> { "Product code already exists." }, null);

        _productRepository.Add(product);

        return (true, new List<string>(), product);
    }

    public (bool ok, List<string> errors)
        UpdateName(string code, string newName, string user)
    {
        var product = _productRepository.Get(code);
        if (product is null)
            return (false, new List<string> { "Product not found." });

        var (isValid, errors) = product.Rename(newName, user);
        if (!isValid) return (false, errors);

        _productRepository.Update(product);

        return (true, new List<string>());
    }

    public (bool ok, List<string> errors)
        Delete(string code)
    {
        if (_productRepository.Get(code) is null)
            return (false, new List<string> { "Product not found." });

        _productRepository.Delete(code);
        return (true, new List<string>());
    }
}