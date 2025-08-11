using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Snafets.TakeItEasy.Api;

namespace Snafets.TakeItEasy.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Optionally configure test services here
            builder.ConfigureServices(services =>
            {
                // Replace real dependencies with test doubles/mocks if needed
                // e.g., swap out DB context for in-memory DB
            });
            return base.CreateHost(builder);
        }
    }
}
