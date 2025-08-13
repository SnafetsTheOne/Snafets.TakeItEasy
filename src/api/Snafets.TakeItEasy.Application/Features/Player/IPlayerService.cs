using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Application.Features.Player;

public interface IPlayerService
{
    Task<PlayerModel?> GetPlayerByIdAsync(Guid id);
    Task<PlayerModel?> LoginAsync(string name, string passwordHash);
    Task<PlayerModel> SignUpAsync(string name, string passwordHash);
}
