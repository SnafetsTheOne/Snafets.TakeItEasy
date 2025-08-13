namespace Snafets.TakeItEasy.Api.Requests;

public class PlayerMoveRequest
{
    public Guid PlayerId { get; set; }
    public int Index { get; set; }
}
