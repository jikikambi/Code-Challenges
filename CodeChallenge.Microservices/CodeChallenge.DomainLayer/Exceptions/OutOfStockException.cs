 namespace CodeChallenge.DomainLayer.Exceptions;

public class OutOfStockException : Exception
{
    public OutOfStockException(string productId) : base($"Product {productId} is out of stock") { }
}