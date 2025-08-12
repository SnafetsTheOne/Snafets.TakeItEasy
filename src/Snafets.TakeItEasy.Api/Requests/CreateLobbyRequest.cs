namespace Snafets.TakeItEasy.Api.Requests;
public class CreateLobbyRequest
{
    public string Name { get; set; }
    public Guid CreatorId { get; set; }
}
