namespace ProcureDesk.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;

    public string Role { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public DateTime CreatedDate { get; private set; }
    public DateTime EditDate { get; private set; }
    public string CreateUser { get; private set; } = null!;
    public string EditUser { get; private set; } = null!;

    private User() { }

    public static (bool isValid, List<string> errors, User? user) Create(
        string username,
        string email,
        string passwordHash,
        string role,
        string actorUser)
    {
        var (isValid, errors) = Validate(username, email, passwordHash, role, actorUser);
        if (!isValid) return (false, errors, null);

        var now = DateTime.UtcNow;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username.Trim(),
            Email = email.Trim(),
            PasswordHash = passwordHash,
            Role = role,
            IsActive = true,
            CreatedDate = now,
            EditDate = now,
            CreateUser = actorUser,
            EditUser = actorUser
        };

        return (true, new List<string>(), user);
    }

    public (bool isValid, List<string> errors) ChangeRole(string newRole, string actorUser)
    {
        var (isValid, errors) = ValidateRole(newRole, actorUser);
        if (!isValid) return (false, errors);

        Role = newRole;
        Touch(actorUser);
        return (true, new List<string>());
    }

    public (bool isValid, List<string> errors) Deactivate(string actorUser)
    {
        if (string.IsNullOrWhiteSpace(actorUser))
            return (false, new List<string> { "User is required." });

        IsActive = false;
        Touch(actorUser);
        return (true, new List<string>());
    }

    public (bool isValid, List<string> errors) ChangePasswordHash(string newPasswordHash, string actorUser)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            return (false, new List<string> { "PasswordHash is required." });

        if (string.IsNullOrWhiteSpace(actorUser))
            return (false, new List<string> { "User is required." });

        PasswordHash = newPasswordHash;
        Touch(actorUser);
        return (true, new List<string>());
    }

    private void Touch(string user)
    {
        EditUser = user;
        EditDate = DateTime.UtcNow;
    }

    private static (bool isValid, List<string> errors) Validate(
        string username, string email, string passwordHash, string role, string actorUser)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(username))
            errors.Add("Username is required.");
        else if (username.Length > 100)
            errors.Add("Username is too long (max 100).");

        if (string.IsNullOrWhiteSpace(email))
            errors.Add("Email is required.");
        else if (email.Length > 200)
            errors.Add("Email is too long (max 200).");

        if (string.IsNullOrWhiteSpace(passwordHash))
            errors.Add("PasswordHash is required.");

        if (string.IsNullOrWhiteSpace(role))
            errors.Add("Role is required.");
        else if (!IsAllowedRole(role))
            errors.Add("Role is invalid.");

        if (string.IsNullOrWhiteSpace(actorUser))
            errors.Add("User is required.");

        return (errors.Count == 0, errors);
    }

    private static (bool isValid, List<string> errors) ValidateRole(string role, string actorUser)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(role))
            errors.Add("Role is required.");
        else if (!IsAllowedRole(role))
            errors.Add("Role is invalid.");

        if (string.IsNullOrWhiteSpace(actorUser))
            errors.Add("User is required.");

        return (errors.Count == 0, errors);
    }

    private static bool IsAllowedRole(string role)
        => role is "Admin" or "Buyer" or "Approver" or "User";
}