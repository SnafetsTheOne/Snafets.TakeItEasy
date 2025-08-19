using Microsoft.AspNetCore.SignalR;
using Snafets.TakeItEasy.Application.Features;

namespace Snafets.TakeItEasy.Api.SignalR;

public class Notifier : INotifier
{
    private readonly IHubContext<UpdatesHub> _hub;
    public Notifier(IHubContext<UpdatesHub> hub) => _hub = hub;

    public Task Broadcast(DateTime dateTime)
    {
        return _hub.Clients.All.SendAsync("broadcast", dateTime);
    }

    public Task NotifyGameStartUpdate(Guid userId, Guid lobbyId, Guid gameId)
    {
        return _hub.Clients.User(userId.ToString()).SendAsync("gameStart", new { lobbyId, gameId });
    }

    public Task NotifyGameUpdate(Guid userId, Guid gameId)
    {
        return _hub.Clients.User(userId.ToString()).SendAsync("gameUpdate", gameId);
    }

    public Task NotifyLobbyUpdateAll(Guid lobbyId)
    {
        return _hub.Clients.All.SendAsync("lobbyUpdate", lobbyId);
    }
}