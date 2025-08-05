using CodeChallenge.ApplicationLayer.Requests.Extensions;
using CodeChallenge.InfrastructureLayer.Services;
using MediatR;
using Order.Service.Api.Application.Mappers;
using Order.Service.Shared.Response;

namespace Order.Service.Api.Application.RequestHandlers.Queries;

public class GetOrderByIdQueryRequestHandler(IOrderRepository repository,
    IOrderResponseMapper mapper) : IRequestHandler<GetOrderByIdQueryRequest, OrderResponse>
{
    public async Task<OrderResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Input.OrderNumber);

        if(order is null)
        {
            request.SetMessage($"Order with ID {request.Input.OrderNumber} not found.");
            return null!;
        }

        request.AddInComingData(order, $"Retrived order `{request.Input.OrderNumber}`");
        var mappedOrder = mapper.MapToResponse(order);
        request.AddOutGoingData(mappedOrder, "Mapped OrderResponse being returned to the user/application");

        return mappedOrder;
    }
}