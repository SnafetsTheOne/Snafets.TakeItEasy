namespace Snafets.TakeItEasy.Domain.Game;

/// <summary>
/// Represents the Take It Easy game domain model.
/// </summary>
public class TakeItEasyGame

{
    public Guid Id { get; set; }
    public List<PlayerBoard> PlayerBoards { get; set; }
    public DrawBag CallerBag { get; set; }

    /// <summary>
    /// Creates a new TakeItEasyGame for the given players.
    /// </summary>
    public TakeItEasyGame(List<Guid> playerIds)
    {
        Id = Guid.NewGuid();
        PlayerBoards = playerIds.Select(id => new PlayerBoard(id)).ToList();
        CallerBag = new DrawBag();
    }

    /// <summary>
    /// Returns true if all player boards are fully filled (no empty spaces).
    /// </summary>
    public bool IsCompleted => PlayerBoards.All(pb => pb.Spaces.All(space => space.PlacedTile != null));

    /// <summary>
    /// Checks if all players have placed the tile currently on top of the draw bag.
    /// If so, removes the top tile from the draw bag and returns true; otherwise, false.
    /// </summary>
    public bool TryAdvanceDrawBagIfAllPlayersPlacedTopTile()
    {
        var topTile = CallerBag.PeekTopTile();
        if (topTile == null)
            return false;

        // Check if all players have placed the top tile
        bool allPlaced = PlayerBoards.All(pb => pb.Spaces.Any(space => space.PlacedTile != null && space.PlacedTile.Id == topTile.Id));
        if (allPlaced)
        {
            // Remove the top tile from the draw bag
            CallerBag.Tiles.Dequeue();
            return true;
        }
        return false;
    }
}