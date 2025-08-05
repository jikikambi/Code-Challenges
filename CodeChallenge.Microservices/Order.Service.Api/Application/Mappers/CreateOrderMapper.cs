using CodeChallenge.DomainLayer.Entities;
using CodeChallenge.DomainLayer.ValueObjects;
using Order.Service.Shared.Request;
using OrderRdm = CodeChallenge.DomainLayer.Entities.Order;

namespace Order.Service.Api.Application.Mappers;

public class CreateOrderMapper : ICreateOrderMapper
{
    public OrderRdm MapToOrder(CreateOrderRequest createOrderRequest)
    {
        var orderItems = createOrderRequest.Items.Select(i =>
            new OrderItem(i.ProductId, i.ProductName, i.ProductAmount, i.ProductPrice)).ToList();

        return new OrderRdm(
            new Address(createOrderRequest.InvoiceAddress),
            new Email(createOrderRequest.InvoiceEmail),
            new CreditCard(createOrderRequest.CreditCardNumber),
            orderItems
        );
    }
}