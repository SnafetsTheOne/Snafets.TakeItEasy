namespace Snafets.TakeItEasy.Domain;

public class PlayerModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? PasswordHash { get; set; }
}
