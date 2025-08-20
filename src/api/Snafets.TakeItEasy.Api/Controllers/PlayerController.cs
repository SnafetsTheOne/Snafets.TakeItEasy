using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Api.Requests;
using Snafets.TakeItEasy.Api.Contracts.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Snafets.TakeItEasy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(IPlayerService playerService, ILogger<PlayerController> logger) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
    {
        logger.LogInformation("POST /api/player/signup: {Name}", request.Name);
        var player = await playerService.SignUpAsync(request.Name, request.PasswordHash);
        return CreatedAtAction(nameof(Login), null, PlayerDto.FromModel(player));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CreatePlayerRequest request)
    {
        logger.LogInformation("POST /api/player/login: {Name}", request.Name);
        var player = await playerService.LoginAsync(request.Name, request.PasswordHash);
        if (player == null)
            return Unauthorized();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, player.Id.ToString()),
            new(ClaimTypes.Name, player.Name)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProps = new AuthenticationProperties
        {
            IsPersistent = true,                           // keep cookie across sessions
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) // match ExpireTimeSpan above
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);
        return Ok(new { ok = true });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        logger.LogInformation("POST /api/player/logout");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { ok = true });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerById(Guid id)
    {
        logger.LogInformation("GET /api/player/{Id}", id);
        var player = await playerService.GetPlayerByIdAsync(id);
        if (player == null)
            return NotFound();
        return Ok(PlayerDto.FromModel(player));
    }

    [HttpGet("me"), Authorize]
    public Task<IActionResult> GetMe()
    {
        ClaimsPrincipal user = HttpContext.User;
        logger.LogInformation("GET /api/player/me");
        if (user?.Identity?.IsAuthenticated != true)
            return Task.FromResult<IActionResult>(Unauthorized());

        var res = new
        {
            id = user.FindFirstValue(ClaimTypes.NameIdentifier),
            name = user.FindFirstValue(ClaimTypes.Name)
        };
        return Task.FromResult<IActionResult>(Ok(res));
    }
}
