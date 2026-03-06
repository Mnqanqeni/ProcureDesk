using System.Data;
using Dapper;
using ProcureDesk.Domain;
using ProcureDesk.Domain.Abstractions;
using ProcureDesk.Infrastructure;

namespace ProcureDesk.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _factory;

    public UserRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public void Insert(User user)
    {
        using var connection = _factory.Create();

        connection.Execute(
            "dbo.user_insert",
            new
            {
                user.Id,
                user.Username,
                user.Email,
                user.PasswordHash,
                user.Role,
                CreateUser = user.CreateUser // match proc param name if needed
            },
            commandType: CommandType.StoredProcedure);
    }

    public bool ExistsByUsername(string username)
    {
        using var connection = _factory.Create();

        // simplest: if proc returns a user row when exists
        var user = connection.QuerySingleOrDefault<User>(
            "dbo.user_getByUsername",
            new { Username = username },
            commandType: CommandType.StoredProcedure);

        return user != null;
    }

    public bool ExistsByEmail(string email)
    {
        using var connection = _factory.Create();

        // you'll need dbo.user_getByEmail proc (below)
        var user = connection.QuerySingleOrDefault<User>(
            "dbo.user_getByEmail",
            new { Email = email },
            commandType: CommandType.StoredProcedure);

        return user != null;
    }

    public User? GetByUsername(string username)
    {
        using var connection = _factory.Create();

        return connection.QuerySingleOrDefault<User>(
            "dbo.user_getByUsername",
            new { Username = username },
            commandType: CommandType.StoredProcedure);
    }
}