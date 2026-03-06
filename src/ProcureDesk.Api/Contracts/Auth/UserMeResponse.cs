namespace ProcureDesk.Api.Contracts.Auth;

public sealed class UserMeResponse
{
    public Guid Id { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Role { get; init; } = null!;
    public bool IsActive { get; init; }
}