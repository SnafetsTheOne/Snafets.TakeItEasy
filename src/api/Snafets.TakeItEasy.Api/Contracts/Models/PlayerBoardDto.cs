using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Contracts.Models;

public class PlayerBoardDto
{
    public Guid PlayerId { get; set; }
    public List<BoardSpaceDto> Spaces { get; set; }
    public int Score { get; set; }
    public bool CanPlay { get; set; }

    public static PlayerBoardDto FromDomain(PlayerBoard playerBoard, Tile? currentTile)
    {
        return new PlayerBoardDto
        {
            PlayerId = playerBoard.PlayerId,
            Spaces = playerBoard.Spaces.Select(BoardSpaceDto.FromDomain).ToList(),
            Score = playerBoard.Score,
            CanPlay = currentTile != null && !playerBoard.Spaces.Any(x => x.PlacedTile?.Id == currentTile?.Id)
        };
    }
}
