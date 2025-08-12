using PlayerModel = Snafets.TakeItEasy.Domain.PlayerModel;

namespace Snafets.TakeItEasy.Application.Features.Player;

public interface IPlayerRepository
{
    Task<PlayerModel> AddPlayerAsync(PlayerModel player);
    Task<PlayerModel?> GetPlayerByIdAsync(Guid id);
}
