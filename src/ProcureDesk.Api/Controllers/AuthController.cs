using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcureDesk.Api.Contracts.Auth;
using ProcureDesk.Application;
using ProcureDesk.Domain.Constants;

namespace ProcureDesk.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserApplicationService _service;
    private readonly IConfiguration _config;

    public AuthController(UserApplicationService service, IConfiguration config)
    {
        _service = service;
        _config = config;
    }

    // POST: /api/auth/register
    // Anyone can register, but role is ALWAYS defaulted to "User" in the application/domain.
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public ActionResult<RegisterResponse> Register([FromBody] RegisterRequest contract)
    {
        var actor = "System"; // later: HttpContext.User.Identity?.Name ?? "System"

        // Register() should default role to Roles.User internally (or pass Roles.User here).
        var (ok, errors, user) = _service.Register(
            contract.Username,
            contract.Email,
            contract.Password,
            actor);

        if (!ok)
            return BadRequest(new { errors });

        var response = new RegisterResponse
        {
            Id = user!.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            CreatedDate = user.CreatedDate
        };

        return Ok(response);
    }

    // POST: /api/auth/login
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        // Best: Login returns (ok, token, error, user). If yours returns only token, see note below.
        var (ok, token, error, user) = _service.Login(request.Username, request.Password);

        if (!ok)
            return Unauthorized(new { error });

        var expiryMinutesText = _config["Jwt:ExpiryMinutes"];
        var expiryMinutes = 120;
        if (!string.IsNullOrWhiteSpace(expiryMinutesText) && int.TryParse(expiryMinutesText, out var parsed))
            expiryMinutes = parsed;

        return Ok(new LoginResponse
        {
            AccessToken = token!,
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(expiryMinutes),
            UserId = user!.Id,
            Username = user.Username,
            Role = user.Role
        });
    }

    // OPTIONAL: Admin-only role assignment endpoint
    // PUT: /api/auth/users/{id}/role
    // Only Admin can assign roles.
    // [Authorize(Roles = Roles.Admin)]
    // [HttpPut("users/{id:guid}/role")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    // public IActionResult AssignRole([FromRoute] Guid id, [FromBody] AssignRoleRequest contract)
    // {
    //     var actor = HttpContext.User.Identity?.Name ?? "System";

    //     var (ok, errors) = _service.AssignRole(id, contract.Role, actor);

    //     if (!ok)
    //         return BadRequest(new { errors });

    //     return NoContent();
    // }
}