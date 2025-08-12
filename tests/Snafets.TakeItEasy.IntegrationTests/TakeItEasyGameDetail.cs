namespace Snafets.TakeItEasy.IntegrationTests;

public class LobbyDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public List<Guid>? PlayerIds { get; set; }
}

public class GameDto
{
    public Guid? Id { get; set; }
    public List<PlayerBoardDto>? PlayerBoards { get; set; }
    public TileDto? NextTile { get; set; }
}

public class PlayerBoardDto
{
    public List<BoardSpaceDto>? Spaces { get; set; }
    public Guid? PlayerId { get; set; }
    public int Score { get; set; }
}

public class BoardSpaceDto
{
    public int? Index { get; set; }
    public TileDto? PlacedTile { get; set; }
}

public class PlayerDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}

public class TileDto
{
    public int? Id { get; set; }
    public int? Vertical { get; set; }
    public int? LeftDiagonal { get; set; }
    public int? RightDiagonal { get; set; }
}

public class DrawBagDto
{
    public List<TileDto>? Tiles { get; set; }
}
