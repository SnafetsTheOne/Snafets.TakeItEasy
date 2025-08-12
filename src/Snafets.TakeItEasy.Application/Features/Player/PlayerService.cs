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
        var player = new PlayerModel { Id = Guid.NewGuid(), Name = name, PasswordHash = passwordHash };
        await _playerRepository.AddPlayerAsync(player);
        return player;
    }

    public async Task<PlayerModel?> GetPlayerByIdAsync(Guid id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id);
    }

    public async Task<PlayerModel?> SignInAsync(string name, string passwordHash)
    {
        var player = await _playerRepository.GetPlayerByNameAndHashAsync(name, passwordHash);
        if (player == null)
            return null;
        return player;
    }
}
