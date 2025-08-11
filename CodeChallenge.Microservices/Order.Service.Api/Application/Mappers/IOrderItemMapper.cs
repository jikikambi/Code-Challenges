using CodeChallenge.DomainLayer.Order;
using Order.Service.Shared.Request;

namespace Order.Service.Api.Application.Mappers;

public interface IOrderItemMapper
{
    List<OrderItem> MapToOrderItem(CreateOrderRequest input);
}