namespace Snafets.TakeItEasy.Domain.Game;

/// <summary>
/// Represents the Take It Easy game domain model.
/// </summary>
public class GameModel
{
    public static Random Rng { get; set; } = new Random();
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<PlayerBoard> PlayerBoards { get; set; }
    public int CurrentMove { get; set; }
    public List<Tile> Bag { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Creates a new TakeItEasyGame for the given players.
    /// </summary>
    public GameModel(List<Guid> playerIds, string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        PlayerBoards = playerIds.Select(id => new PlayerBoard(id)).ToList();
        CurrentMove = 0;

        var tileList = new List<Tile>();
        int id = 1;
        foreach (var v in Tile.ValidVerticals)
        {
            foreach (var l in Tile.ValidLeftDiagonals)
            {
                foreach (var r in Tile.ValidRightDiagonals)
                {
                    tileList.Add(new Tile(id++, v, l, r));
                }
            }
        }

        // Shuffle the tiles
        int n = tileList.Count;
        while (n > 1)
        {
            n--;
            int k = Rng.Next(n + 1);
            var value = tileList[k];
            tileList[k] = tileList[n];
            tileList[n] = value;
        }

        Bag = tileList;
    }

    public Tile CurrentTile => Bag[CurrentMove];
    public bool IsCompleted => CurrentMove >= 19;

    /// <summary>
    /// Checks if all players have placed the tile currently on top of the draw bag.
    /// If so, removes the top tile from the draw bag and returns true; otherwise, false.
    /// </summary>
    public bool TryAdvanceDrawBagIfAllPlayersPlacedCurrentTile()
    {
        // Check if all players have placed the current tile
        bool allPlaced = PlayerBoards.All(pb => pb.Spaces.Any(space => space.PlacedTile != null && space.PlacedTile.Id == CurrentTile.Id));
        if (allPlaced)
        {
            CurrentMove++;
            return true;
        }
        return false;
    }
}