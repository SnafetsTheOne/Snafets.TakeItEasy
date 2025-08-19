namespace Snafets.TakeItEasy.Application.Features
{
    public interface INotifier
    {
        Task Broadcast(DateTime dateTime);
        Task NotifyGameUpdate(Guid userId, Guid gameId);
        Task NotifyLobbyUpdateAll(Guid lobbyId);
        Task NotifyGameStartUpdate(Guid userId, Guid lobbyId, Guid gameId);
    }
}