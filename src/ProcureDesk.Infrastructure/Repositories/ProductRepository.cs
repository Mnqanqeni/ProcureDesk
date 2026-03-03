using System.Data;
using Dapper;
using ProcureDesk.Domain;
using ProcureDesk.Domain.Abstractions;

namespace ProcureDesk.Infrastructure;

public sealed class ProductRepository : IProductRepository
{
    private readonly DbConnectionFactory _factory;

    public ProductRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    private IDbConnection CreateConnection() => _factory.Create();

    public IEnumerable<Product> GetAll()
    {
    
        using var connection = CreateConnection();
        return connection.Query<Product>("dbo.product_getAll", commandType: CommandType.StoredProcedure);
    
    }
    public Product? Get(string code)
    {

        using var connection = CreateConnection();
        var parameters = new { Code = code };
        return connection.QuerySingleOrDefault<Product>("dbo.product_getByCode", parameters, commandType: CommandType.StoredProcedure);
    }

    public void Add(Product product)
    {
        using var connection = CreateConnection();
        var parameters = new
        {
            product.Code,
            product.Name,
            User=product.CreateUser,
    
        };
        connection.Execute("dbo.product_insert", parameters, commandType: CommandType.StoredProcedure);
    }

    public void Update(Product product)
    {
     
        using var connection = CreateConnection();
        var parameters = new
        {
            product.Code,
            product.Name,
            User=product.EditUser,
    
        };
        connection.Execute("dbo.product_updateName", parameters, commandType: CommandType.StoredProcedure);
    }

    public void Delete(string code)
    {

        using var connection = CreateConnection();
        var parameters = new { Code = code };
        connection.Execute("dbo.product_delete", parameters, commandType: CommandType.StoredProcedure);
    }
}