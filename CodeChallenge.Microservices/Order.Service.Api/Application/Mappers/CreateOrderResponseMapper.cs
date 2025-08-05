using Order.Service.Shared.Response;
using OrderRdm = CodeChallenge.DomainLayer.Entities.Order;

namespace Order.Service.Api.Application.Mappers;

public class CreateOrderResponseMapper : ICreateOrderResponseMapper
{
    public CreateOrderResponse MapToResponse(OrderRdm order)
    {
        return new CreateOrderResponse(order.Id);
    }
}