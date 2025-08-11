using CodeChallenge.DomainLayer.Dtos;
using Order.Service.Shared.Response;
using OrderRdm = CodeChallenge.DomainLayer.Order.Order;

namespace Order.Service.Api.Application.Mappers;

public class OrderResponseMapper : IOrderResponseMapper
{
    public OrderResponse MapToResponse(OrderRdm order)
    {
        return new OrderResponse(
            order.Id,
            order.InvoiceAddress.Value,
            order.InvoiceEmail.Value,
            order.CreditCard.Value,
            [.. order.Items.Select(i => new OrderItemDto(
                i.ProductId,
                i.ProductName,
                i.ProductAmount,
                i.ProductPrice))],
            order.CreatedAt
        );
    }
}