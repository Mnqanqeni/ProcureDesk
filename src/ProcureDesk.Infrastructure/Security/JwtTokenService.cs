using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProcureDesk.Domain;
using ProcureDesk.Domain.Abstractions;

namespace ProcureDesk.Infrastructure.Security;

public sealed class JwtTokenService : ITokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public string Generate(User user)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));

        var secret = _config["Jwt:Secret"];
        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("Missing configuration: Jwt:Secret");

        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];

        var expiryMinutesText = _config["Jwt:ExpiryMinutes"];
        var expiryMinutes = 120; // default 2 hours
        if (!string.IsNullOrWhiteSpace(expiryMinutesText) && int.TryParse(expiryMinutesText, out var parsed))
            expiryMinutes = parsed;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}