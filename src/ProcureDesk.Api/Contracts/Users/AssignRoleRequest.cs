namespace ProcureDesk.Api.Contracts.Users;

public sealed class AssignRoleRequest
{
    public string Role { get; init; } = null!;
}