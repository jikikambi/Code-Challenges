using AutoFixture;
using CodeChallenge.DomainLayer.Order;
using CodeChallenge.InfrastructureLayer.Services;
using CodeChallenge.InfrastructureLayer.UnitTests.Custom;

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

        _fixture.ResetDatabase();

        _autoFixture = new Fixture();

        // Register all customizations
        _autoFixture.Customize(new EmailCustomization());
        _autoFixture.Customize(new AddressCustomization());
        _autoFixture.Customize(new CreditCardCustomization());
        _autoFixture.Customize(new OrderItemCustomization());
        _autoFixture.Customize(new OrderCustomization());
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