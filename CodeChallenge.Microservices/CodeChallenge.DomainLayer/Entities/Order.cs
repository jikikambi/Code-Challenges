using CodeChallenge.DomainLayer.ValueObjects;

namespace CodeChallenge.DomainLayer.Entities;

public class Order
{
    private readonly List<OrderItem> _items = [];

    private Order() { }

    public Guid Id { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Address InvoiceAddress { get; private set; }
    public Email InvoiceEmail { get; private set; }
    public CreditCard CreditCard { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Order(Address address, Email email, CreditCard creditCard, List<OrderItem> items)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("Order must contain at least one item.");

        Id = Guid.NewGuid(); // Domain generates the identity
        InvoiceAddress = address ?? throw new ArgumentNullException(nameof(address));
        InvoiceEmail = email ?? throw new ArgumentNullException(nameof(email));
        CreditCard = creditCard ?? throw new ArgumentNullException(nameof(creditCard));
        CreatedAt = DateTime.UtcNow;

        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    public void AddItem(OrderItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _items.Add(item);
    }
}