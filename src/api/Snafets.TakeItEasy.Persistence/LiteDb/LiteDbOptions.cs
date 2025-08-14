namespace Snafets.TakeItEasy.Persistence.LiteDb;

public class LiteDbOptions
{
    public string DatabasePath { get; set; } = "takeiteasy.db";
    public string PlayersCollectionName { get; set; } = "players";
    public string LobbiesCollectionName { get; set; } = "lobbies";
    public string GamesCollectionName { get; set; } = "games";
}