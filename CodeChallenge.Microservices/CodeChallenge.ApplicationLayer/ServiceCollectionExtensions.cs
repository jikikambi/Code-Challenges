using CodeChallenge.ApplicationLayer.Behaviors;
using CodeChallenge.ApplicationLayer.Behaviors.Validators;
using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Models;
using CodeChallenge.ApplicationLayer.Tracking.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.ApplicationLayer;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds MediatR services and request handlers for API.
    /// </summary>
    public static IServiceCollection AddMediatRForApi<
        TRegister,
        TRequest,
        TResponse,
        TInput>(
        this IServiceCollection services,
        IConfiguration configuration,
        bool enableValidationBehavior = true,
        bool isTrackingEnabled = true)
        where TRequest : ITrackingRequestBase<TInput>
        where TInput : class
        => AddCommonMediatRForApi<TRegister, TRequest, TResponse, TInput>(services, configuration, enableValidationBehavior, isTrackingEnabled);

    private static IServiceCollection AddCommonMediatRForApi<
        TRegister,
        TRequest,
        TResponse,
        TInput>(
        IServiceCollection services,
        IConfiguration configuration,
        bool enableValidationBehavior,
        bool isTrackingEnabled)
        where TRequest : ITrackingRequestBase<TInput>
        where TInput : class
    {
        services.AddValidatorsFromAssembly(typeof(TRegister).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(TRegister).Assembly, typeof(ServiceCollectionExtensions).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<TRequest, TResponse>), typeof(LoggingBehavior<TRequest, TResponse, TInput>));

            if (isTrackingEnabled)
            {
                cfg.AddBehavior(typeof(IPipelineBehavior<TRequest, TResponse>), typeof(TrackingBehavior<TRequest, TResponse, TInput>));
            }

            services.AddApiValidationBehavior<TRequest, TResponse, TInput>(cfg);
        });

        // Should be called after the assembly scanning for validators and MediatR behaviors
        services.AddTrackingService(configuration);
        return services;
    }

    private static void AddApiValidationBehavior<
        TRequest,
        TResponse,
        TInput>(
        this IServiceCollection services,
        MediatRServiceConfiguration cfg)
        where TRequest : ITrackingRequestBase<TInput>
    {
        if (services.Any(service =>
            service.ServiceType.IsGenericType &&
            service.ServiceType.FullName == typeof(IValidator<TInput>).FullName))
        {
            cfg.AddBehavior(typeof(IPipelineBehavior<TRequest, TResponse>), typeof(ApiValidationBehavior<TRequest, TResponse, TInput>));
        }
    }

    /// <summary>
    /// Registers the Serilog-based implementation of ITrackingService.
    /// </summary>
    public static void AddTrackingService(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind TrackingOptions from configuration
        services.Configure<TrackingOptions>(configuration.GetSection("Tracking"));

        // Register IElasticClient
        services.AddSingleton<IElasticClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TrackingOptions>>().Value;

            var settings = new ConnectionSettings(new Uri(options.ElasticsearchUrl));

            if (!string.IsNullOrWhiteSpace(options.Username) && !string.IsNullOrWhiteSpace(options.Password))
            {
                settings = settings.BasicAuthentication(options.Username, options.Password);
            }

            if (!string.IsNullOrWhiteSpace(options.IndexPrefix))
            {
                settings = settings.DefaultIndex(options.IndexPrefix);
            }

            return new ElasticClient(settings);
        });

        // Register TrackingService
        services.AddSingleton<ITrackingService, TrackingService>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger>();
            var elasticClient = sp.GetRequiredService<IElasticClient>();
            var options = sp.GetRequiredService<IOptions<TrackingOptions>>();
            return new TrackingService(logger, elasticClient, options);
        });
    }
}