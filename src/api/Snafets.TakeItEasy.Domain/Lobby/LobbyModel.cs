namespace Snafets.TakeItEasy.Domain.Lobby;

public class LobbyModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid> PlayerIds { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
