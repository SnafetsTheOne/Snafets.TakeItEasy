using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Contracts.Models;

public class PlayerBoardDto
{
    public Guid PlayerId { get; set; }
    public List<BoardSpaceDto> Spaces { get; set; }
    public int Score { get; set; }

    public static PlayerBoardDto FromDomain(PlayerBoard playerBoard)
    {
        return new PlayerBoardDto
        {
            PlayerId = playerBoard.PlayerId,
            Spaces = playerBoard.Spaces.Select(BoardSpaceDto.FromDomain).ToList(),
            Score = playerBoard.Score
        };
    }
}
