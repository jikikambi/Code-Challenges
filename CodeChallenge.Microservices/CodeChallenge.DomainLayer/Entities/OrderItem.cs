namespace CodeChallenge.DomainLayer.Entities;

public class OrderItem
{
    private OrderItem() { }

    public string ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int ProductAmount { get; private set; }
    public decimal ProductPrice { get; private set; }

    public OrderItem(string productId, string productName, int productAmount, decimal productPrice)
    {
        ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        ProductAmount = productAmount;
        ProductPrice = productPrice;
    }    
}