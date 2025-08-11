namespace Snafets.TakeItEasy.Domain
{
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
        public TakeItEasyGame(List<Player> players)
        {
            Id = Guid.NewGuid();
            PlayerBoards = new List<PlayerBoard>();
            foreach (var player in players)
            {
                PlayerBoards.Add(new PlayerBoard(player));
            }
            CallerBag = new DrawBag();
        }

        /// <summary>
        /// Returns true if all player boards are fully filled (no empty spaces).
        /// </summary>
        public bool IsCompleted => PlayerBoards != null && PlayerBoards.All(pb => pb.Spaces != null && pb.Spaces.All(space => space.PlacedTile != null));
    }
}