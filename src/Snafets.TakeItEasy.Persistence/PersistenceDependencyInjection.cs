using Microsoft.Extensions.DependencyInjection;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Persistence.Player;

namespace Snafets.TakeItEasy.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services)
    {
        // Register persistence layer services here
        services.AddSingleton<IGameRepository, InMemoryGameRepository>();
        services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
        services.AddSingleton<ILobbyRepository, InMemoryLobbyRepository>();

        return services;
    }
}
