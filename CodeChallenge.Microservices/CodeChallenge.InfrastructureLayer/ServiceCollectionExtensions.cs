using CodeChallenge.ApplicationLayer;
using CodeChallenge.DomainLayer.Order.Services;
using CodeChallenge.InfrastructureLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using OrderRdm = CodeChallenge.DomainLayer.Order.Order;

namespace CodeChallenge.InfrastructureLayer;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnection = configuration.GetConnectionString("CodeChallengeConnStr");
        services.AddScoped<IOrderRepository, OrderRepository>()
            .AddDbContext<OrderDbContext>(options => options.UseSqlServer(sqlConnection, options => 
            {
                options.MigrationsAssembly("CodeChallenge.InfrastructureLayer");
                options.EnableRetryOnFailure();
            }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IEventStoreRepository<OrderRdm>, EventStoreRepository<OrderRdm>>();
        services.AddScoped<IInventoryService, InventoryService>();
    }
}