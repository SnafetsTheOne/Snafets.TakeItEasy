using Microsoft.AspNetCore.Mvc;
using Snafets.TakeItEasy.Application;
using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<TakeItEasyGame>> CreateGame([FromBody] List<Player> players)
        {
            var game = await _gameService.CreateGameAsync(players);
            return Ok(game);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TakeItEasyGame?>> GetGame(Guid id)
        {
            var game = await _gameService.GetGameAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost("move")]
        public async Task<ActionResult<bool>> AddPlayerMove(Guid gameId, Guid playerId, int index, int tileId)
        {
            var result = await _gameService.AddPlayerMoveAsync(gameId, playerId, index, tileId);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<TakeItEasyGame>>> GetAllGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }

        [HttpGet("player/{playerId}")]
        public async Task<ActionResult<List<TakeItEasyGame>>> LoadGameForPlayer(Guid playerId)
        {
            var games = await _gameService.LoadGameForPlayerAsync(playerId);
            return Ok(games);
        }
    }
}
