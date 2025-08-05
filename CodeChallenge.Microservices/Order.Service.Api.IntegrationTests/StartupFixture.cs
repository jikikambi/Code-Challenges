using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Order.Service.Api.Client;

/// <summary>
/// This class is used to share the startup fixture between tests.
/// For more information see; https://xunit.net/docs/shared-context
/// </summary>
public class StartupFixture : IDisposable
{
    public IServiceProvider ServiceProvider;

    public StartupFixture()
    {
        // Build the configuration
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        // Create a new service collection and register the necessary services
        var serviceCollection = new ServiceCollection();

        // Add Refit client
        serviceCollection.AddOrderServiceApiRefitClient();        

        // Build the service provider
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
    public void Dispose()
    {
        ServiceProvider = null!;
    }
}