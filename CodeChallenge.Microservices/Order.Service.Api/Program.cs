using Order.Service.Api;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilogLogging(builder.Configuration);
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

var app = builder.RegisterServices();
    
await app.SetupMiddleware();

await app.RunAsync();