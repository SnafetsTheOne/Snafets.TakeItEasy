using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Api.Requests;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Snafets.TakeItEasy.Api.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        logger.LogInformation("GET /api/lobby/{Id}", id);
        var lobby = await lobbyService.GetLobbyAsync(id);
        if (lobby == null)
        {
            return NotFound();
        }
        return Ok(lobby);
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> CreateLobby([FromBody] CreateLobbyRequest request)
    {
        logger.LogInformation("POST /api/lobby: {Name} {CreatorId}", request.Name, request.CreatorId);
        var lobby = await lobbyService.CreateLobbyAsync(request.Name, request.CreatorId);
        return CreatedAtAction(nameof(GetLobbyById), new { id = lobby.Id }, lobby);
    }

    [HttpPost("{id}/join"), Authorize]
    public async Task<IActionResult> JoinLobby(Guid id, [FromBody] PlayerActionRequest request)
    {
        logger.LogInformation("POST /api/lobby/{Id}/join {PlayerId}", id, request.PlayerId);
        var lobby = await lobbyService.UpdateLobby_AddPlayerAsync(id, request.PlayerId);
        if (lobby == null)
        {
            return NotFound();
        }
        return Ok(lobby);
    }

    [HttpPost("{id}/leave"), Authorize]
    public async Task<IActionResult> LeaveLobby(Guid id, [FromBody] PlayerActionRequest request)
    {
        logger.LogInformation("POST /api/lobby/{Id}/leave {PlayerId}", id, request.PlayerId);
        var success = await lobbyService.UpdateLobby_RemovePlayerAsync(id, request.PlayerId);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("{id}/start"), Authorize]
    public async Task<IActionResult> StartGame(Guid id)
    {
        logger.LogInformation("POST /api/lobby/{Id}/start", id);
        var playerId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var game = await lobbyService.DeleteLobbyAndStartGameAsync(id, playerId);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(GameDto.FromDomain(game, playerId));
    }

    /// <summary>
    /// Deletes a lobby by id.
    /// </summary>
    [HttpDelete("{id}"), Authorize]
    public async Task<IActionResult> DeleteLobby(Guid id)
    {
        logger.LogInformation("DELETE /api/lobby/{Id}", id);
        var playerId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await lobbyService.DeleteLobbyAsync(id, playerId);
        return NoContent();
    }
}
