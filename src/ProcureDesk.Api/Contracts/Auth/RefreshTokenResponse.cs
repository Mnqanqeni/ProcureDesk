namespace ProcureDesk.Api.Contracts.Auth;

public sealed class RefreshTokenResponse
{
    public string AccessToken { get; init; } = null!;
    public DateTime ExpiresAtUtc { get; init; }
    public string RefreshToken { get; init; } = null!;
}