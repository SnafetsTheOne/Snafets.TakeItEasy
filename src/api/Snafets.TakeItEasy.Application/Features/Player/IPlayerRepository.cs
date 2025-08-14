using PlayerModel = Snafets.TakeItEasy.Domain.PlayerModel;

namespace Snafets.TakeItEasy.Application.Features.Player;

public interface IPlayerRepository
{
    Task<PlayerModel> SavePlayerAsync(PlayerModel player);
    Task<PlayerModel?> GetPlayerAsync(Guid id);
    Task<PlayerModel?> GetPlayerAsync(string name, string passwordHash);
}
