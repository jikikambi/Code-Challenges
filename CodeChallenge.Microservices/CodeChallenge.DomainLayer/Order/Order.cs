using System.ComponentModel.DataAnnotations.Schema;
using CodeChallenge.DomainLayer.Common;
using CodeChallenge.DomainLayer.Dtos;
using CodeChallenge.DomainLayer.Order.Events;
using CodeChallenge.DomainLayer.ValueObjects;

namespace CodeChallenge.DomainLayer.Order;

public class Order : IEventSourcedAggregate
{
    private int _version = -1; // version of last applied event
    private readonly List<OrderItem> _items = [];

    [NotMapped]
    private readonly List<DomainEvent> _uncommittedEvents = [];

    public Order() { } 

    public Guid Id { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Address InvoiceAddress { get; private set; }
    public Email InvoiceEmail { get; private set; }
    public CreditCard CreditCard { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int Version => _version;

    /// <summary>
    /// Expose uncommitted events to be persisted by repository
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DomainEvent> GetUncommittedEvents() => _uncommittedEvents;
    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

    /// <summary>
    /// Factory method to create a new order - raises OrderCreatedDomainEvent
    /// </summary>
    /// <param name="address"></param>
    /// <param name="email"></param>
    /// <param name="creditCard"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Order Create(Address address, Email email, CreditCard creditCard, List<OrderItem> items)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("Order must contain at least one item.");

        if (email == null || string.IsNullOrWhiteSpace(email.Value) || !email.Value.Contains("@"))
            throw new ArgumentException("Invalid email address.");

        var order = new Order();

        var orderCreatedEvent = new OrderCreatedDomainEvent(
            Guid.NewGuid(),
            address.Value,
            email.Value,
            creditCard.Value,
            [.. items.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.ProductAmount, i.ProductPrice))]);

        order.RaiseEvent(orderCreatedEvent);
        return order;
    }

    /// <summary>
    /// Apply event to mutate state
    /// </summary>
    /// <param name="event"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void ApplyEvent(DomainEvent @event)
    {
        switch (@event)
        {
            case OrderCreatedDomainEvent e:
                Id = e.OrderId;
                InvoiceAddress = new Address(e.InvoiceAddress);
                InvoiceEmail = new Email(e.Email);
                CreditCard = new CreditCard(e.CreditCard);
                _items.Clear();
                foreach (var itemDto in e.Items)
                {
                    _items.Add(new OrderItem(itemDto.ProductId, itemDto.ProductName, itemDto.ProductAmount, itemDto.ProductPrice));
                }
                CreatedAt = e.Timestamp;
                break;

            // Add other event types here when extending the model

            default:
                throw new InvalidOperationException($"Unsupported event type: {@event.GetType().Name}");
        }

        _version++; // increment version on every applied event
    }

    /// <summary>
    /// Raise a new domain event and mark it uncommitted
    /// Thus applies and tracks new events.
    /// </summary>
    /// <param name="event"></param>
    private void RaiseEvent(DomainEvent @event)
    {
        ApplyEvent(@event);               // update aggregate state immediately
        _uncommittedEvents.Add(@event);  // keep track to persist
    }

    /// <summary>
    /// Rebuild aggregate from history of events (e.g. from event store)
    /// Thus replays past events to rebuild.
    /// </summary>
    /// <param name="history"></param>
    public void LoadFromHistory(IEnumerable<DomainEvent> history)
    {
        foreach (var @event in history)
        {
            ApplyEvent(@event);
        }
        ClearUncommittedEvents(); // after rebuild, no uncommitted events
    }
        
    public decimal CalculateTotal()
    {
        return _items.Sum(i => i.ProductPrice * i.ProductAmount);
    }
}