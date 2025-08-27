import * as signalR from "@microsoft/signalr";

const baseUrl = window.__ENV__?.BACKEND_URL ?? "";

// api: src/api/Snafets.TakeItEasy.Api/SignalR/UpdatesHub.cs
export function buildConnection() {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${baseUrl}hubs/updates`, {
      withCredentials: true
    })
    .withAutomaticReconnect({
      nextRetryDelayInMilliseconds(ctx) {
        if (!ctx) return 0;
        const tries = ctx.previousRetryCount + 1;
        const delays = [2000, 5000, 10000, 20000, 30000];
        return delays[Math.min(tries - 1, delays.length - 1)];
      },
    })
    .build();
}