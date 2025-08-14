using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Application.Features.Game;

public class GameService : IGameService
{
    private readonly IGameRepository _repository;
    private readonly INotifier _notifier;

    public GameService(IGameRepository repository, INotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public async Task<GameModel> CreateGameAsync(List<Guid> playerIds, string name)
    {
        var game = new GameModel(playerIds, name);
        await _repository.SaveGameAsync(game);
        return game;
    }

    public async Task<List<GameModel>> GetGamesByPlayerIdAsync(Guid playerId)
    {
        return (await _repository.GetAllGamesAsync(playerId)).ToList();
    }

    public async Task<GameModel?> GetGameAsync(Guid id)
    {
        return await _repository.GetGameAsync(id);
    }

    public async Task<GameModel?> AddPlayerMoveAsync(Guid gameId, Guid playerId, int index)
    {
        var game = await _repository.GetGameAsync(gameId);
        if (game is null)
            return null;

        var playerBoard = game.PlayerBoards.Find(pb => pb.PlayerId == playerId);
        if (playerBoard is null)
            return null;

        var moveResult = playerBoard.TryAddTileAtIndex(game.CurrentTile, index);
        if (!moveResult) return null;

        game.TryAdvanceDrawBagIfAllPlayersPlacedCurrentTile();

        await _repository.SaveGameAsync(game);

        foreach (var otherPlayerBoard in game.PlayerBoards.Where(pb => pb.PlayerId != playerId))
        {
            await _notifier.NotifyGameUpdate(otherPlayerBoard.PlayerId, game.Id);
        }

        return game;
    }
}
