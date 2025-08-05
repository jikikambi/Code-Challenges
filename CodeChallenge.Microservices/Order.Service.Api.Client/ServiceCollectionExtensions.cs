using CodeChallenge.ApplicationLayer;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Api.Client;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public static class ServiceCollectionExtensions
{
    public static void AddOrderServiceApiRefitClient(this IServiceCollection services)
    {
        services.AddRefitClient<IOrderServiceApiClient>()
        .ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://localhost:7191"); 
        });
    }
}