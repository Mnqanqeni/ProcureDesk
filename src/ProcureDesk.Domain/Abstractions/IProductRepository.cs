namespace ProcureDesk.Domain.Abstractions
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product? Get(string code);
        void Add(Product product);
        void Update(Product product);
        void Delete(string code);
    }


}