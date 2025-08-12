using Microsoft.Extensions.DependencyInjection;
using Snafets.TakeItEasy.Application.Features.Game;

namespace Snafets.TakeItEasy.Persistence
{
    public static class PersistenceDependencyInjection
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services)
        {
            // Register persistence layer services here
            services.AddSingleton<IGameRepository, InMemoryGameRepository>();
            return services;
        }
    }
}
