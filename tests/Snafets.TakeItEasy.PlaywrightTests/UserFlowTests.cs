using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace Snafets.TakeItEasy.PlaywrightTests;

public class UserFlowTests : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _page;

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        _page = await _browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null) await _page.CloseAsync();
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }

    [Fact]
    public async Task SignUp_CreateLobby_StartGame_VerifyGameStarted()
    {
        var username = $"user{Guid.NewGuid():N}";
        var password = "password123";

        await _page!.GotoAsync("http://localhost:3000/signup");
        await _page.FillAsync("#name", username);
        await _page.FillAsync("#password", password);
        await _page.GetByRole(AriaRole.Button, new() { Name = "Sign Up" }).ClickAsync();

        await _page.FillAsync("#lobbyName", "Playwright Lobby");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Create Lobby" }).ClickAsync();

        await _page.ClickAsync("[aria-label^='Open lobby']");

        await _page.GetByRole(AriaRole.Button, new() { Name = "Start" }).ClickAsync();

        await _page.WaitForURLAsync(url => url.Contains("/game/"));
        await _page.WaitForSelectorAsync("text=Score");
    }
}
