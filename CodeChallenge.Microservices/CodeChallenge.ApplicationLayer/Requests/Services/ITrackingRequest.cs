using MediatR;

namespace CodeChallenge.ApplicationLayer.Requests.Services;

internal interface ITrackingRequest<TInput> : IRequest, ITrackingRequestBase<TInput>
{ }

internal interface ITrackingRequest<TInput, out TResponse> : IRequest<TResponse>, ITrackingRequestBase<TInput>
{ }