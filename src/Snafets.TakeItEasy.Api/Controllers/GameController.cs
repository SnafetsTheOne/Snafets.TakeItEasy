using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Api.Requests;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController(IGameService gameService, ILogger<GameController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TakeItEasyGame>> CreateGame([FromBody] CreateGameRequest createGameRequest)
    {
        logger.LogInformation("POST /api/game: {PlayerIds}", string.Join(",", createGameRequest.PlayerIds));
        var game = await gameService.CreateGameAsync(createGameRequest.PlayerIds);
        return Ok(game);
    }

    [HttpGet]
    public async Task<ActionResult<List<TakeItEasyGame>>> GetAllGames()
    {
        logger.LogInformation("GET /api/game");
        var games = await gameService.GetAllGamesAsync();
        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TakeItEasyGame?>> GetGame(Guid id)
    {
        logger.LogInformation("GET /api/game/{id}", id);
        var game = await gameService.GetGameAsync(id);
        if (game == null) return NotFound();
        return Ok(game);
    }

    [HttpPost("{id}/move")]
    public async Task<ActionResult<bool>> AddPlayerMove(Guid id, [FromBody] PlayerMoveRequest request)
    {
        logger.LogInformation("POST /api/game/{id}/move {PlayerId} {Index}", id, request.PlayerId, request.Index);
        var result = await gameService.AddPlayerMoveAsync(id, request.PlayerId, request.Index);
        if (!result)
        {
            return BadRequest("Invalid move.");
        }
        return Ok(result);
    }
}
