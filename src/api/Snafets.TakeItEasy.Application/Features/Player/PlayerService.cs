using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Application.Features.Player;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<PlayerModel> SignUpAsync(string name, string passwordHash)
    {
        var player = new PlayerModel
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            PasswordHash = passwordHash
        };
        await _playerRepository.SavePlayerAsync(player);
        return player;
    }

    public async Task<PlayerModel?> GetPlayerByIdAsync(Guid id)
    {
        return await _playerRepository.GetPlayerAsync(id);
    }

    public async Task<PlayerModel?> LoginAsync(string name, string passwordHash)
    {
        var player = await _playerRepository.GetPlayerAsync(name, passwordHash);
        if (player == null)
            return null;
        return player;
    }
}
