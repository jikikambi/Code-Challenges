using Order.Service.Shared.Response;
using OrderRdm = CodeChallenge.DomainLayer.Entities.Order;

namespace Order.Service.Api.Application.Mappers;

public interface IOrderResponseMapper
{
    OrderResponse MapToResponse(OrderRdm order);
}