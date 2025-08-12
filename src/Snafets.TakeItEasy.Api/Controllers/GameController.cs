using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Domain;
using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController(IGameService gameService, ILogger<GameController> logger) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<TakeItEasyGame>> CreateGame([FromBody] List<Player> players)
        {
            var game = await gameService.CreateGameAsync(players);
            return Ok(game);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TakeItEasyGame?>> GetGame(Guid id)
        {
            var game = await gameService.GetGameAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost("move")]
        public async Task<ActionResult<bool>> AddPlayerMove(Guid gameId, Guid playerId, int index)
        {
            logger.LogInformation("Adding move for player {PlayerId} in game {GameId} at index {Index}", playerId, gameId, index);
            var result = await gameService.AddPlayerMoveAsync(gameId, playerId, index);
            if(!result)
            {
                return BadRequest("Invalid move.");
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<TakeItEasyGame>>> GetAllGames()
        {
            var games = await gameService.GetAllGamesAsync();
            return Ok(games);
        }

        [HttpGet("player/{playerId}")]
        public async Task<ActionResult<List<TakeItEasyGame>>> LoadGameForPlayer(Guid playerId)
        {
            var games = await gameService.LoadGameForPlayerAsync(playerId);
            return Ok(games);
        }
    }
}
