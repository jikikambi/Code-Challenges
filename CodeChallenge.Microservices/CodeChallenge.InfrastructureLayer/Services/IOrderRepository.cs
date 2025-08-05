using CodeChallenge.DomainLayer.Entities;

namespace CodeChallenge.InfrastructureLayer.Services;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid orderNumber);
    Task<Guid> AddAsync(Order order);
    Task DeleteAsync(Guid orderNumber);
}