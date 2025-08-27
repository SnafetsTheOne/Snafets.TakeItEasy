using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Api.Contracts.Models;
using Snafets.TakeItEasy.Api.Requests;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Controllers;

// data access: src/ui/src/data-access/game.js
[ApiController]
[Route("api/[controller]")]
public class GameController(IGameService gameService, ILogger<GameController> logger) : ControllerBase
{
    [HttpGet, Authorize]
    public async Task<ActionResult<List<GameModel>>> GetAllGames([FromQuery] bool includeCompleted = false)
    {
        logger.LogInformation("GET /api/game");
        var playerId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var games = await gameService.GetGamesByPlayerIdAsync(playerId, includeCompleted);
        return Ok(games.Select(x => GameDto.FromDomain(x, playerId)));
    }

    [HttpGet("{id}"), Authorize]
    public async Task<ActionResult<GameModel?>> GetGame(Guid id)
    {
        logger.LogInformation("GET /api/game/{id}", id);
        var game = await gameService.GetGameAsync(id);
        if (game == null) return NotFound();
        var playerId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (game.PlayerBoards.Any(x => x.PlayerId == playerId))
        {
            return Ok(GameDto.FromDomain(game, playerId));
        }
        return Forbid();
    }

    [HttpPost("{id}/move"), Authorize]
    public async Task<ActionResult<bool>> AddPlayerMove(Guid id, [FromBody] PlayerMoveRequest request)
    {
        logger.LogInformation("POST /api/game/{id}/move {PlayerId} {Index}", id, request.PlayerId, request.Index);
        var playerId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (playerId != request.PlayerId)
        {
            return Forbid();
        }
        var updatedGame = await gameService.AddPlayerMoveAsync(id, request.PlayerId, request.Index);
        if (updatedGame == null)
        {
            return BadRequest("Invalid move.");
        }
        return Ok(GameDto.FromDomain(updatedGame, playerId));
    }
}
