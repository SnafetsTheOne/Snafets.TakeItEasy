using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Api.Requests;

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

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
    {
        var player = await _playerService.CreatePlayerAsync(request.Name, request.PasswordHash);
        return CreatedAtAction(nameof(GetPlayerById), new { id = player.Id }, player);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerById(Guid id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        if (player == null)
            return NotFound();
        return Ok(player);
    }
}
