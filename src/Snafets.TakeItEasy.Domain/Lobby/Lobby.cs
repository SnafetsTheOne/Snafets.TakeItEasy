namespace Snafets.TakeItEasy.Domain.Lobby;

public class LobbyModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid> PlayerIds { get; set; } = new List<Guid>();
}
