namespace ProcureDesk.Api.Contracts.Auth;

public sealed class LoginRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}