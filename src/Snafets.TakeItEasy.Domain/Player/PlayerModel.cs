namespace Snafets.TakeItEasy.Domain;

public class PlayerModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
}
