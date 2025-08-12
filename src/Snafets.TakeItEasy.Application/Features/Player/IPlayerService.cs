using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Application.Features.Player;

public interface IPlayerService
{
    Task<PlayerModel> CreatePlayerAsync(string name, string passwordHash);
    Task<PlayerModel?> GetPlayerByIdAsync(Guid id);
    Task<PlayerModel?> SignInAsync(string name, string passwordHash);
}
