using Microsoft.Extensions.DependencyInjection;
using Snafets.TakeItEasy.Application.Features.Game;

namespace Snafets.TakeItEasy.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // Register application layer services here
            services.AddScoped<IGameService, GameService>();
            return services;
        }
    }
}
