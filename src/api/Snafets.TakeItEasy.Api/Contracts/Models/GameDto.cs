using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Contracts.Models;

public class GameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public PlayerBoardDto MyBoard { get; set; }
    public List<PlayerBoardDto> OtherPlayerBoards { get; set; }
    public TileDto? NextTile { get; set; }
    public bool MyTurn { get; set; }
    public bool IsCompleted { get; set; }

    public static GameDto FromDomain(GameModel game, Guid playerId)
    {
        var isCompleted = game.CallerBag.Tiles.Count <= 8;
        var nextTile = isCompleted ? null : game.CallerBag.PeekTopTile();
        var myBoard = PlayerBoardDto.FromDomain(game.PlayerBoards.First(p => p.PlayerId == playerId));
        return new GameDto
        {
            Id = game.Id,
            Name = game.Name,
            MyBoard = myBoard,
            OtherPlayerBoards = game.PlayerBoards.Where(p => p.PlayerId != playerId).Select(PlayerBoardDto.FromDomain).ToList(),
            NextTile = nextTile != null ? TileDto.FromDomain(nextTile) : null,
            MyTurn = nextTile != null && !myBoard.Spaces.Any(x => x.PlacedTile?.Id == nextTile?.Id),
            IsCompleted = isCompleted
        };
    }
}