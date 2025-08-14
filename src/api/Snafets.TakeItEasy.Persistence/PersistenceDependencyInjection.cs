
using System;
using Microsoft.Extensions.DependencyInjection;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Persistence.LiteDb;
using Snafets.TakeItEasy.Persistence.Player;

namespace Snafets.TakeItEasy.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services)
    {
        if (Environment.GetEnvironmentVariable("USE_LITEDB") == "false")
        {
            // Register in-memory persistence layer services here
            services.AddSingleton<IGameRepository, InMemoryGameRepository>();
            services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
            services.AddSingleton<ILobbyRepository, InMemoryLobbyRepository>();
        }
        else
        {
            // Register LiteDB persistence layer services here
            services.AddOptions<LiteDbOptions>()
                .BindConfiguration("LiteDb")
                .ValidateOnStart();

            services.AddSingleton<IGameRepository, LiteDbGameRepository>();
            services.AddSingleton<IPlayerRepository, LiteDbPlayerRepository>();
            services.AddSingleton<ILobbyRepository, LiteDbLobbyRepository>();
        }

        return services;
    }
}
