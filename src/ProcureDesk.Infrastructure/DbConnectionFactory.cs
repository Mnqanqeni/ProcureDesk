using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient; 

namespace ProcureDesk.Infrastructure;

public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default")!;
    }

    public IDbConnection Create() => new SqlConnection(_connectionString);
}