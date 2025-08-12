using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Api.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
    {
        var player = await _playerService.CreatePlayerAsync(request.Name, request.PasswordHash);
        return CreatedAtAction(nameof(SignIn), null, player);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] CreatePlayerRequest request)
    {
        var player = await _playerService.SignInAsync(request.Name, request.PasswordHash);
        if (player == null)
            return Unauthorized();
        return Ok(player);
    }
}
