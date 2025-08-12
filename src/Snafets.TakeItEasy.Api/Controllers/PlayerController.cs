using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Api.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Snafets.TakeItEasy.Domain;
using Microsoft.Extensions.Logging;

namespace Snafets.TakeItEasy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController(IPlayerService playerService, ILogger<PlayerController> logger) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
    {
        logger.LogInformation("POST /api/player/signup: {Name}", request.Name);
        var player = await playerService.CreatePlayerAsync(request.Name, request.PasswordHash);
        return CreatedAtAction(nameof(SignIn), null, player);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] CreatePlayerRequest request)
    {
        logger.LogInformation("POST /api/player/signin: {Name}", request.Name);
        var player = await playerService.SignInAsync(request.Name, request.PasswordHash);
        if (player == null)
            return Unauthorized();
        return Ok(player);
    }
}
