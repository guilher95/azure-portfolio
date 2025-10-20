using AuthService.Infrastructure;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


#if DEBUG
// Fijamos el entorno a Development para cualquier ejecución desde VS/Debug
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Environments.Development);
Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", Environments.Development);
#endif

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"ENV: {builder.Environment.EnvironmentName}");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Configuration.GetValue<bool>("ApplyMigrations"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok(new { service = "auth-service", status = "ok" }));
app.MapHealthChecks("/health");

app.Run();
