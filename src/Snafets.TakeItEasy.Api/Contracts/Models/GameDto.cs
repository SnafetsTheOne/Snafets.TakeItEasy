using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Contracts.Models;

public class GameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<PlayerBoardDto> PlayerBoards { get; set; }
    public TileDto? NextTile { get; set; }
    public bool IsCompleted { get; set; }

    public static GameDto FromDomain(GameModel game)
    {
        var nextTile = game.CallerBag.PeekTopTile();
        return new GameDto
        {
            Id = game.Id,
            Name = game.Name,
            PlayerBoards = game.PlayerBoards.Select(PlayerBoardDto.FromDomain).ToList(),
            NextTile = nextTile != null ? TileDto.FromDomain(nextTile) : null,
            IsCompleted = game.CallerBag.Tiles.Count == 8
        };
    }
}