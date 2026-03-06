namespace ProcureDesk.Api.Contracts.Auth;

public sealed class RegisterRequest
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}