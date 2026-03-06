namespace ProcureDesk.Api.Contracts.Auth;

public sealed class RegisterResponse
{
    public Guid Id { get; init; }

    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;

    public string Role { get; init; } = null!;

    public DateTime CreatedDate { get; init; }
}