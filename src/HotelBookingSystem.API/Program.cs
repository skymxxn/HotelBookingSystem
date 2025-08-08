using HotelBookingSystem.API.DependencyInjection;
using HotelBookingSystem.API.Extensions;
using HotelBookingSystem.Application.Common.Mappings;
using HotelBookingSystem.Application.DependencyInjection;
using HotelBookingSystem.Infrastructure.DependencyInjection;
using HotelBookingSystem.Persistence.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddPresentation(builder.Configuration);

MapsterConfig.RegisterMappings();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("HotelBookingSystem is starting up on server {Url}",
    builder.Configuration["AppInfo:BaseUrl"]);

await app.MigrateAndSeedAsync();

app.Run();