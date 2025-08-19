using CodeChallenge.DomainLayer.Order;
using CodeChallenge.DomainLayer.ValueObjects;
using CodeChallenge.InfrastructureLayer.Services;
using FluentAssertions;

namespace CodeChallenge.InfrastructureLayer.UnitTests.Services;

public class EventStoreRepositoryTests : IClassFixture<OrderDbContextFixture>
{
    private readonly OrderDbContextFixture _fixture;
    private readonly EventStoreRepository<Order> _repository;

    public EventStoreRepositoryTests(OrderDbContextFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetDatabase();

        _repository = new EventStoreRepository<Order>(_fixture.Context);
    }

    [Fact]
    public async Task Should_Save_And_Restore_EventSourcedAggregate()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var address = new Address("123 Sample Street, 90402 Berlin");
        var email = new Email("customer@example.com");
        var card = new CreditCard("1234-5678-9101-1121");

        var orderItem = new OrderItem("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M);
        var order = Order.Create(address, email, card, [orderItem]);

        // Act
        await _repository.SaveAsync(order);
        var rehydrated = await _repository.GetByIdAsync(order.Id);

        // Assert
        rehydrated.Should().NotBeNull();
        rehydrated!.Id.Should().Be(order.Id);
        rehydrated.Version.Should().Be(order.Version);
        rehydrated.Items.Should().HaveCount(1);
        rehydrated.Items.First().ProductName.Should().Be("Gaming Laptop");
    }
}