namespace ProcureDesk.Domain.Abstractions;

public interface IUserRepository
{
    void Insert(User user);

    bool ExistsByUsername(string username);
    bool ExistsByEmail(string email);

    User? GetByUsername(string username); 
}