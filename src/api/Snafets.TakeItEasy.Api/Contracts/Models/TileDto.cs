using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Api.Contracts.Models;

public class TileDto
{
    public int Id { get; set; }
    public int Vertical { get; set; }
    public int LeftDiagonal { get; set; }
    public int RightDiagonal { get; set; }

    public static TileDto FromDomain(Tile tile)
    {
        return new TileDto
        {
            Id = tile.Id,
            Vertical = tile.Vertical,
            LeftDiagonal = tile.LeftDiagonal,
            RightDiagonal = tile.RightDiagonal
        };
    }
}
