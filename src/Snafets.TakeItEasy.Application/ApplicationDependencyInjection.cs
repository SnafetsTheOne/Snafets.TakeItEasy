using Microsoft.Extensions.DependencyInjection;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Snafets.TakeItEasy.Application.Features.Player;

namespace Snafets.TakeItEasy.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // Register application layer services here
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<ILobbyService, LobbyService>();
            return services;
        }
    }
}
