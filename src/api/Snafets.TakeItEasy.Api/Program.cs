using Microsoft.AspNetCore.Authentication.Cookies;
using Snafets.TakeItEasy.Application;
using Snafets.TakeItEasy.Api.SignalR;
using Snafets.TakeItEasy.Persistence;
using Snafets.TakeItEasy.Api.Services;
using Snafets.TakeItEasy.Application.Features;
using Snafets.TakeItEasy.Api;

var builder = WebApplication.CreateBuilder(args);

static bool IsApiOrSignalR(HttpRequest req)
{
    // treat JSON, API paths, and SignalR negotiation/ws as non-HTML
    if (req.Path.StartsWithSegments("/hubs")) return true;
    var accept = req.Headers.Accept.ToString() ?? "";
    return accept.Contains("application/json", StringComparison.OrdinalIgnoreCase);
}

builder.Logging.AddConsole();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<AllowedOrigins>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins!.Origins.ToArray()) // React dev server
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
                if (IsApiOrSignalR(ctx.Request))
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                ctx.Response.Redirect(ctx.RedirectUri);
                return Task.CompletedTask;            },
            OnRedirectToAccessDenied = ctx =>
            {
                if (IsApiOrSignalR(ctx.Request))
                {
                    ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }
                ctx.Response.Redirect(ctx.RedirectUri);
                return Task.CompletedTask;            }
        };
    });
builder.Services.AddSignalR();
builder.Services.AddAuthorization();

// Add OpenAPI/Swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DI for Application and Persistence layers
builder.Services.AddApplicationDependencies();
builder.Services.AddPersistenceDependencies();

// Add controllers
builder.Services.AddControllers();

builder.Services.AddSingleton<INotifier, Notifier>();
builder.Services.AddHostedService<BroadcastService>();

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
app.MapHub<UpdatesHub>("/hubs/updates").RequireAuthorization();

await app.RunAsync();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
