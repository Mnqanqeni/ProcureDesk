namespace ProcureDesk.Api.Contracts.Users;

public sealed class DeactivateUserRequest
{
    public bool IsActive { get; init; }
}