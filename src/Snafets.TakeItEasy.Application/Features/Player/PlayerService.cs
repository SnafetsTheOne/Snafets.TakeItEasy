using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Application.Features.Player;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<PlayerModel> CreatePlayerAsync(string name, string passwordHash)
    {
        var player = new PlayerModel { Name = name, PasswordHash = passwordHash };
        await _playerRepository.AddPlayerAsync(player);
        return player;
    }

    public async Task<PlayerModel?> GetPlayerByIdAsync(Guid id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id);
    }
}
