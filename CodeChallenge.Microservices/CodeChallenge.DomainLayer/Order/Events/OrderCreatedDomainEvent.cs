using CodeChallenge.DomainLayer.Common;
using CodeChallenge.DomainLayer.Dtos;

namespace CodeChallenge.DomainLayer.Order.Events;

public sealed class OrderCreatedDomainEvent : DomainEvent
{
    public Guid OrderId => AggregateId; // Expose it explicitly for clarity

    public string InvoiceAddress { get; init; }
    public string Email { get; init; }
    public string CreditCard { get; init; }
    public List<OrderItemDto> Items { get; init; } = [];

    public OrderCreatedDomainEvent(
        Guid orderId,
        string invoiceAddress,
        string email,
        string creditCard,
        List<OrderItemDto> items)
    {
        AggregateId = orderId;
        InvoiceAddress = invoiceAddress;
        Email = email;
        CreditCard = creditCard;
        Items = items;
    }
}