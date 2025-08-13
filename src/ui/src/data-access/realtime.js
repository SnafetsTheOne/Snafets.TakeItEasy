import * as signalR from "@microsoft/signalr";

const baseUrl = window.__ENV__?.BACKEND_URL ?? "";

export function buildConnection() {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${baseUrl}hubs/updates`, {
      withCredentials: true
    })
    .withAutomaticReconnect()
    .build();
}