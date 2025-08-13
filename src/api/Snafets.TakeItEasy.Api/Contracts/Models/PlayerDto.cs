using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Api.Contracts.Models;

public class PlayerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public static PlayerDto FromModel(PlayerModel model)
    {
        return new PlayerDto
        {
            Id = model.Id,
            Name = model.Name
        };
    }
}