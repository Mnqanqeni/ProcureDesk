namespace ProcureDesk.Api.Contracts.Auth;

public sealed class LoginResponse
{
    public string AccessToken { get; init; } = null!;
    public DateTime ExpiresAtUtc { get; init; }

    public Guid UserId { get; init; }
    public string Username { get; init; } = null!;
    public string Role { get; init; } = null!;
}