using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Api.Requests;
using Snafets.TakeItEasy.Application.Features.Lobby;

namespace Snafets.TakeItEasy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LobbyController : ControllerBase
{
    private readonly ILobbyService _lobbyService;

    public LobbyController(ILobbyService lobbyService)
    {
        _lobbyService = lobbyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLobbies()
    {
        var lobbies = await _lobbyService.GetAllLobbiesAsync();
        return Ok(lobbies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLobbyById(Guid id)
    {
        var lobby = await _lobbyService.GetLobbyAsync(id);
        if (lobby == null)
        {
            return NotFound();
        }
        return Ok(lobby);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLobby([FromBody] CreateLobbyRequest request)
    {
        var lobby = await _lobbyService.CreateLobbyAsync(request.Name, request.CreatorId);
        return CreatedAtAction(nameof(GetLobbyById), new { id = lobby.Id }, lobby);
    }

    [HttpPost("{id}/join")]
    public async Task<IActionResult> JoinLobby(Guid id, [FromBody] PlayerActionRequest request)
    {
        var lobby = await _lobbyService.UpdateLobby_AddPlayerAsync(id, request.PlayerId);
        if (lobby == null)
        {
            return NotFound();
        }
        return Ok(lobby);
    }

    [HttpPost("{id}/leave")]
    public async Task<IActionResult> LeaveLobby(Guid id, [FromBody] PlayerActionRequest request)
    {
        var success = await _lobbyService.UpdateLobby_RemovePlayerAsync(id, request.PlayerId);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartGame(Guid id)
    {
        var game = await _lobbyService.DeleteLobbyAndStartGameAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }
}
