namespace ProcureDesk.Domain.Abstractions;

public interface ITokenService
{
    string Generate(User user);
}