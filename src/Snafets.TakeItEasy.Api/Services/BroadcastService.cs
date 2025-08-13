using Snafets.TakeItEasy.Application.Features;

namespace Snafets.TakeItEasy.Api.Services;

public class BroadcastService : BackgroundService
{
    private readonly INotifier _notifier;

    public BroadcastService(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);

            await _notifier.Broadcast(DateTime.UtcNow);
        }
    }
}