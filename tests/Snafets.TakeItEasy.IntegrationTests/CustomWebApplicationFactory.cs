using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

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
