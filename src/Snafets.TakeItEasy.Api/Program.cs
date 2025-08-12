using Microsoft.AspNetCore.Authentication.Cookies;
using Snafets.TakeItEasy.Application;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5124", "http://localhost:3000") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "app.auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax; // Lax is a good default for CSRF protection + UX
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // use HTTPS in prod
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // session length
        // No LoginPath redirect since we're building an API; we'll return 401s
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

// Add OpenAPI/Swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DI for Application and Persistence layers
builder.Services.AddApplicationDependencies();
builder.Services.AddPersistenceDependencies();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Use CORS middleware
app.UseCors("AllowFrontend");

// Use controllers
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var player1 = await app.Services.GetRequiredService<IPlayerRepository>().AddPlayerAsync(new Snafets.TakeItEasy.Domain.PlayerModel()
{
	Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
	Name = "Alice",
	PasswordHash = "hash1"
});

await app.Services.GetRequiredService<IGameRepository>().SaveGameAsync(new Snafets.TakeItEasy.Domain.Game.GameModel(new List<Guid>{player1.Id}, "name"));
await app.Services.GetRequiredService<ILobbyRepository>().AddLobbyAsync("Test Lobby", player1.Id);
await app.RunAsync();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
