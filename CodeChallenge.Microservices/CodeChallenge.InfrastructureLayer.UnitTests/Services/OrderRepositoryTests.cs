using AutoFixture;
using CodeChallenge.DomainLayer.Entities;
using CodeChallenge.InfrastructureLayer.Services;

namespace CodeChallenge.InfrastructureLayer.UnitTests.Services;

public class OrderRepositoryTests : IClassFixture<OrderDbContextFixture>
{
    private readonly OrderDbContextFixture _fixture;
    private readonly OrderRepository _repository;
    private readonly Fixture _autoFixture;

    public OrderRepositoryTests(OrderDbContextFixture fixture)
    {
        _fixture = fixture;
        _repository = new OrderRepository(_fixture.Context);

        // Clear DB
        _fixture.ResetDatabase();

        // Set up AutoFixture with customizations
        _autoFixture = new Fixture();
        _autoFixture.Customize(new EmailCustomization());
    }

    [Fact]
    public async Task AddAsync_Should_Add_Order()
    {
        var order = _autoFixture.Create<Order>();

        var result = await _repository.AddAsync(order);

        Assert.Equal(order.Id, result);

        var savedOrder = await _fixture.Context.Orders.FindAsync(order.Id);
        Assert.NotNull(savedOrder);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Order_When_Exists()
    {
        var order = _autoFixture.Create<Order>();
        _fixture.Context.Orders.Add(order);
        await _fixture.Context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(order.Id);

        Assert.NotNull(result);
        Assert.Equal(order.Id, result!.Id);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Order_When_Exists()
    {
        var order = _autoFixture.Create<Order>();
        _fixture.Context.Orders.Add(order);
        await _fixture.Context.SaveChangesAsync();

        await _repository.DeleteAsync(order.Id);

        var deletedOrder = await _fixture.Context.Orders.FindAsync(order.Id);
        Assert.Null(deletedOrder);
    }
}