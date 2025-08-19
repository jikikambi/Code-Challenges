namespace CodeChallenge.DomainLayer.Order.Services;

public interface IInventoryService
{
    Task<bool> IsInStockAsync(string productId, int quantity, CancellationToken cancellationToken);
}