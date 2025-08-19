using CodeChallenge.DomainLayer.Order.Services;

namespace CodeChallenge.DomainLayer.Order.Specifications;

/// <summary>
/// Ensures all products are in stock
/// </summary>
/// <param name="inventoryService"></param>
public class ProductInStockSpec(IInventoryService inventoryService)
{
    public async Task<bool> IsSatisfiedBy(IEnumerable<OrderItem> items)
    {
        foreach (var item in items)
        {
            if (!await inventoryService.IsInStockAsync(item.ProductId, item.ProductAmount, CancellationToken.None))
                return false;
        }

        return await Task.FromResult(true);
    }
}