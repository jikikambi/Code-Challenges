using CodeChallenge.DomainLayer.Order;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.InfrastructureLayer.Services;

public class OrderRepository(OrderDbContext context) : IOrderRepository
{
    public async Task<Guid> AddAsync(Order order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
        return order.Id;
    }

    public Task<Order?> GetByIdAsync(Guid orderNumber)
    {
        return context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderNumber);
    }

    public async Task DeleteAsync(Guid orderNumber)
    {
        var entity = await context.Orders.FindAsync(orderNumber);
        if (entity is not null)
        {
            context.Orders.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}