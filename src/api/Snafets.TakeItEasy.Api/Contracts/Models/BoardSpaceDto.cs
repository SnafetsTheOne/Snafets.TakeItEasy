using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Contracts.Models;

public class BoardSpaceDto
{
    public int Index { get; set; }
    public TileDto? PlacedTile { get; set; }

    public static BoardSpaceDto FromDomain(BoardSpace boardSpace)
    {
        return new BoardSpaceDto
        {
            Index = boardSpace.Index,
            PlacedTile = boardSpace.PlacedTile != null ? TileDto.FromDomain(boardSpace.PlacedTile) : null
        };
    }
}
