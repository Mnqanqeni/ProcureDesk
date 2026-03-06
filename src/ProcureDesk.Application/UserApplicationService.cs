using ProcureDesk.Domain.Abstractions;
using ProcureDesk.Domain;
using ProcureDesk.Domain.Constants;

namespace ProcureDesk.Application;
public class UserApplicationService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public UserApplicationService(
        IUserRepository repository,
        IPasswordHasher hasher,
        ITokenService tokenService)
    {
        _repository = repository;
        _hasher = hasher;
        _tokenService = tokenService;
    }

     public (bool isSuccessful, List<string> errors, User? user) Register(
        string username,
        string email,
        string password,
        string actor)
    {
        var errors = new List<string>();

        // 1) Check duplicates (application rule)
        if (_repository.ExistsByUsername(username))
            errors.Add("Username is already taken.");

        if (_repository.ExistsByEmail(email))
            errors.Add("Email is already taken.");

        if (errors.Count > 0)
            return (false, errors, null);

        // 2) Hash password
        var passwordHash = _hasher.Hash(password);

        // 3) Domain create + validation
        var (isValid, domainErrors, user) =
            User.Create(username, email, passwordHash, Roles.User, actor);

        if (!isValid)
            return (false, domainErrors, null);

        // 4) Persist
        _repository.Insert(user!);

        return (true, new(), user);
    }


    public (bool ok, string? token, string? error, User? user) Login(
        string username,
        string password)
    {
        var user = _repository.GetByUsername(username);

        if (user == null)
            return (false, null, "Invalid credentials", null);

        if (!_hasher.Verify(password, user.PasswordHash))
            return (false, null, "Invalid credentials", null);

        if (!user.IsActive)
            return (false, null, "User is inactive", null);

        var token = _tokenService.Generate(user);

        return (true, token, null, user);
    }
}