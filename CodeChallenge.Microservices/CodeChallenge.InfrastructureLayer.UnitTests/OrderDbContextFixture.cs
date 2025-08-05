using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.InfrastructureLayer.UnitTests;

public class OrderDbContextFixture : IDisposable
{
    public OrderDbContext Context { get; private set; }

    public OrderDbContextFixture()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        Context = new OrderDbContext(options);
    }

    public void ResetDatabase()
    {
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}