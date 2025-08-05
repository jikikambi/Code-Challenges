using CodeChallenge.ApplicationLayer;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Api;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public static class SerilogConfigurationExtensions
{
    public static void ConfigureSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            
            .WriteTo.Logger(lc => lc
                .Filter.ByExcluding(Matching.WithProperty<object>("IsTracking", v => v is bool b && b))
                .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day))
            
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty<object>("IsTracking", v => v is bool b && b))
                .WriteTo.File(path:"logs/tracking-.json",
                rollingInterval: RollingInterval.Day,
                formatter: new Serilog.Formatting.Json.JsonFormatter(renderMessage: true)))

            .CreateLogger();

        hostBuilder.UseSerilog();
    }
}