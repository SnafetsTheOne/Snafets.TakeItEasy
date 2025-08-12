using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Api.Requests;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Microsoft.Extensions.Logging;

namespace Snafets.TakeItEasy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LobbyController(ILobbyService lobbyService, ILogger<LobbyController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllLobbies()
    {
        logger.LogInformation("GET /api/lobby");
        var lobbies = await lobbyService.GetAllLobbiesAsync();
        return Ok(lobbies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLobbyById(Guid id)
    {
        logger.LogInformation("GET /api/lobby/{id}", id);
        var lobby = await lobbyService.GetLobbyAsync(id);
        if (lobby == null)
        {
            return NotFound();
        }
        return Ok(lobby);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLobby([FromBody] CreateLobbyRequest request)
    {
        logger.LogInformation("POST /api/lobby: {Name} {CreatorId}", request.Name, request.CreatorId);
        var lobby = await lobbyService.CreateLobbyAsync(request.Name, request.CreatorId);
        return CreatedAtAction(nameof(GetLobbyById), new { id = lobby.Id }, lobby);
    }

    [HttpPost("{id}/join")]
    public async Task<IActionResult> JoinLobby(Guid id, [FromBody] PlayerActionRequest request)
    {
        logger.LogInformation("POST /api/lobby/{id}/join {PlayerId}", id, request.PlayerId);
        var lobby = await lobbyService.UpdateLobby_AddPlayerAsync(id, request.PlayerId);
        if (lobby == null)
        {
            return NotFound();
        }
        return Ok(lobby);
    }

    [HttpPost("{id}/leave")]
    public async Task<IActionResult> LeaveLobby(Guid id, [FromBody] PlayerActionRequest request)
    {
        logger.LogInformation("POST /api/lobby/{id}/leave {PlayerId}", id, request.PlayerId);
        var success = await lobbyService.UpdateLobby_RemovePlayerAsync(id, request.PlayerId);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartGame(Guid id)
    {
        logger.LogInformation("POST /api/lobby/{id}/start", id);
        var game = await lobbyService.DeleteLobbyAndStartGameAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }
}
