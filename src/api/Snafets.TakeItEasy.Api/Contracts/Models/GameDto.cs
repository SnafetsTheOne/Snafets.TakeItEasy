using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Contracts.Models;

public class GameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<PlayerBoardDto> Boards { get; set; }
    public TileDto? CurrentTile { get; set; }
    public bool MyTurn { get; set; }
    public bool IsCompleted { get; set; }

    public static GameDto FromDomain(GameModel game, Guid playerId)
    {
        var currentTile = game.IsCompleted ? null : game.CurrentTile;
        return new GameDto
        {
            Id = game.Id,
            Name = game.Name,
            Boards = game.PlayerBoards
                .OrderByDescending(x => x.PlayerId == playerId)
                .Select(x => PlayerBoardDto.FromDomain(x, currentTile))
                .ToList(),
            CurrentTile = currentTile != null ? TileDto.FromDomain(currentTile) : null,
            IsCompleted = game.IsCompleted
        };
    }
}