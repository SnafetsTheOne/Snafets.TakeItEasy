using Snafets.TakeItEasy.Application;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

// Add CORS services
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});
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
app.UseCors("AllowAll");

// Use controllers
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

var player1 = await app.Services.GetRequiredService<IPlayerRepository>().AddPlayerAsync(new Snafets.TakeItEasy.Domain.PlayerModel()
{
	Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
	Name = "Alice",
	PasswordHash = "hash1"
});

await app.Services.GetRequiredService<IGameRepository>().SaveGameAsync(new Snafets.TakeItEasy.Domain.Game.TakeItEasyGame(new List<Guid>{player1.Id}));

await app.RunAsync();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
