using CodeChallenge.DomainLayer.Order;
using Order.Service.Shared.Request;

namespace Order.Service.Api.Application.Mappers;

public class OrderItemMapper : IOrderItemMapper
{
    public List<OrderItem> MapToOrderItem(CreateOrderRequest input)
    {
        //return new OrderItem("productId", "productName", 2, 20);
        return
        [
            new OrderItem("productId", "productName", 2, 20)
        ];
    }
}