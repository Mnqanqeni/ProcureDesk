namespace ProcureDesk.Api.Contracts.Auth;

public sealed class ChangePasswordRequest
{
    public string CurrentPassword { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}