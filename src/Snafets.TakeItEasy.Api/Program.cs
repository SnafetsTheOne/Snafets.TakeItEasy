using Snafets.TakeItEasy.Application;
using Snafets.TakeItEasy.Persistence;

var builder = WebApplication.CreateBuilder(args);

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

// Use controllers
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
