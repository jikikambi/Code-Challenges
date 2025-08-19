using CodeChallenge.DomainLayer.Order;
using CodeChallenge.DomainLayer.ValueObjects;
using Order.Service.Shared.Request;
using OrderRdm = CodeChallenge.DomainLayer.Order.Order;
using OrderItemRdm = CodeChallenge.DomainLayer.Order.OrderItem;

namespace Order.Service.Api.Application.Mappers;

public class CreateOrderMapper : ICreateOrderMapper
{
    public OrderRdm MapToOrder(CreateOrderRequest createOrderRequest)
    {
        //var orderItems = createOrderRequest.Items.Select(i =>
        //    new OrderItem(i.ProductId, i.ProductName, i.ProductAmount, i.ProductPrice)).ToList();

        //var address = new Address(createOrderRequest.InvoiceAddress);
        //var email = new Email(createOrderRequest.InvoiceEmail);
        //var creditCard = new CreditCard(createOrderRequest.CreditCardNumber);

        //return OrderRdm.Create(address, email, creditCard, orderItems);
        return OrderRdm.Create(
            new Address(createOrderRequest.InvoiceAddress),
            new Email(createOrderRequest.InvoiceEmail),
            new CreditCard(createOrderRequest.CreditCardNumber),
            [.. createOrderRequest.Items.Select(oi => new OrderItemRdm(
                oi.ProductId,
                oi.ProductName,
                oi.ProductAmount,
                oi.ProductPrice))]
        );
    }
}