using Order.Service.Shared.Request;
using OrderRdm = CodeChallenge.DomainLayer.Entities.Order;

namespace Order.Service.Api.Application.Mappers;

public interface ICreateOrderMapper
{
    OrderRdm MapToOrder(CreateOrderRequest createOrderRequest);
}