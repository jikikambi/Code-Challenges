using Order.Service.Shared.Response;
using OrderRdm = CodeChallenge.DomainLayer.Entities.Order;

namespace Order.Service.Api.Application.Mappers;

public interface ICreateOrderResponseMapper
{
    CreateOrderResponse MapToResponse(OrderRdm order);
}