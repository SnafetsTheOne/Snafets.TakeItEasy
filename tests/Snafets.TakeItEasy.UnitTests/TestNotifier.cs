using Snafets.TakeItEasy.Application.Features;

namespace Snafets.TakeItEasy.UnitTests;

public class TestNotifier : INotifier
{
    public Task Broadcast(DateTime dateTime)
    {
        return Task.CompletedTask;
    }

    public Task NotifyGameStartUpdate(Guid userId, Guid lobbyId, Guid gameId)
    {
        return Task.CompletedTask;
    }

    public Task NotifyGameUpdate(Guid playerId, Guid gameId)
    {
        return Task.CompletedTask;
    }

    public Task NotifyLobbyUpdate(Guid userId, Guid lobbyId)
    {
        return Task.CompletedTask;
    }
}
