using MediatR;
using Order.Service.Shared.Request;
using Order.Service.Shared.Response;
using Refit;

namespace Order.Service.Api.Client;

public interface IOrderServiceApiClient
{
    [Post("/orders")]
    Task<ApiResponse<CreateOrderResponse>> CreateOrderAsync([Body] CreateOrderRequest request, CancellationToken cancellationToken = default);

    [Get("/orders/{orderNumber}")]
    Task<ApiResponse<OrderDetailResponse>> GetOrderAsync(Guid orderNumber, CancellationToken cancellationToken = default);

    [Delete("/orders/{orderNumber}")]
    Task<ApiResponse<Unit>> DeleteOrderAsync(Guid orderNumber, CancellationToken cancellationToken = default);
}