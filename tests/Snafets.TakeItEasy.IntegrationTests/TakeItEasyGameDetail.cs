namespace Snafets.TakeItEasy.IntegrationTests
{
    public class TakeItEasyGameDetail
    {
        public Guid? Id { get; set; }
        public List<PlayerBoardDetail>? PlayerBoards { get; set; }
        public DrawBagDetail? CallerBag { get; set; }
    }

    public class PlayerBoardDetail
    {
        public List<BoardSpaceDetail>? Spaces { get; set; }
        public PlayerDetail? Player { get; set; }
        public int Score { get; set; }
    }

    public class BoardSpaceDetail
    {
        public int? Index { get; set; }
        public TileDetail? PlacedTile { get; set; }
    }

    public class PlayerDetail
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }

    public class TileDetail
    {
        public int? Id { get; set; }
        public int? Vertical { get; set; }
        public int? LeftDiagonal { get; set; }
        public int? RightDiagonal { get; set; }
    }

    public class DrawBagDetail
    {
        public List<TileDetail>? Tiles { get; set; }
    }
}
