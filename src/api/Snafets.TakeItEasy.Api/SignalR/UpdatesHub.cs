using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Snafets.TakeItEasy.Api.SignalR;

// data-access: src/ui/src/data-access/realtime.js
public class UpdatesHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
            Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        return base.OnConnectedAsync();
    }
}