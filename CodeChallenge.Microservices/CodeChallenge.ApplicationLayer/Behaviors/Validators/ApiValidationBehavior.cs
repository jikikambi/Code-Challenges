using CodeChallenge.ApplicationLayer.Requests.Services;
using FluentValidation;
using MediatR;

namespace CodeChallenge.ApplicationLayer.Behaviors.Validators;

/// <summary>
/// It ensures that incoming requests are validated before they proceed to the request handler.
/// </summary>
/// <typeparam name="TRequest">Must implement IRequestProcessorBase<TInput>, ensuring it contains tracking-related properties:</typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <typeparam name="TInput">data being validated</typeparam>
/// <param name="validator"></param>
public class ApiValidationBehavior<TRequest, TResponse, TInput>(IValidator<TInput> validator) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITrackingRequestBase<TInput>
{
    /// <summary>
    /// Validates request.Input using FluentValidation:
    /// - Calls ValidateAndThrowAsync(), which: 
    ///   - Throws an exception if validation fails.
    ///   - Proceeds to next() if validation passes.
    /// Passes control to the next pipeline behavior or request handler if validation succeeds.   
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        ArgumentNullException.ThrowIfNull(validator);

        await validator.ValidateAndThrowAsync(request.Input, cancellationToken);
        return await next();
    }
}