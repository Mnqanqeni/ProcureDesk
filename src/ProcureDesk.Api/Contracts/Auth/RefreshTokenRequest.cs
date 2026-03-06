namespace ProcureDesk.Api.Contracts.Auth;

public sealed class RefreshTokenRequest
{
    public string RefreshToken { get; init; } = null!;
}