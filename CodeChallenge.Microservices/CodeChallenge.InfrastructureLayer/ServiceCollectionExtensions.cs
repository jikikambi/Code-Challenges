using CodeChallenge.InfrastructureLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.InfrastructureLayer;

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
    }
}