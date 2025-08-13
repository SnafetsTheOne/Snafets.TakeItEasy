import * as signalR from "@microsoft/signalr";

export function buildConnection() {
  return new signalR.HubConnectionBuilder()
    .withUrl(`http://localhost:5124/hubs/updates`, {
      withCredentials: true
    })
    .withAutomaticReconnect()
    .build();
}