using CodeChallenge.ApplicationLayer.Behaviors;
using CodeChallenge.ApplicationLayer.Behaviors.Validators;
using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddSingleton<ITrackingService, TrackingService>(sp =>
        {
            var logger = sp.GetRequiredService<Serilog.ILogger>();
            return new TrackingService(logger);
        });
    }
}