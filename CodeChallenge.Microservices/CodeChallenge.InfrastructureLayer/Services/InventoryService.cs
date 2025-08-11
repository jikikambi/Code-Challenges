using CodeChallenge.DomainLayer.Order.Services;

namespace CodeChallenge.InfrastructureLayer.Services;

public class InventoryService : IInventoryService
{
    public async Task<bool> IsInStockAsync(string productId, int productAmount, CancellationToken cancellationToken)
    {
        // TODO: Replace with actual DB/API call
        return await Task.FromResult(true); // Just assume everything is in stock
    }
}