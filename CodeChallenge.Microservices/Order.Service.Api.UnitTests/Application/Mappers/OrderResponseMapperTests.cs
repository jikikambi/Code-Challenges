using CodeChallenge.DomainLayer.ValueObjects;
using FluentAssertions;
using Order.Service.Api.Application.Mappers;
using OrderRdm = CodeChallenge.DomainLayer.Order.Order;

namespace Order.Service.Api.UnitTests.Application.Mappers;

public class OrderResponseMapperTests
{
    [Fact]
    public void Should_Map_OrderRdm_To_OrderResponse_Correctly()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        var order = OrderRdm.Create(
            new Address("123 Sample Street, 90402 Berlin"),
            new Email("customer@example.com"),
            new CreditCard("1234-5678-9101-1121"),
            [
                new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M),
                new("5e8fad5b-d9cb-469f-a165-70867728950d", "Gaming Controller", 1, 29.99m)
            ]
        );

        typeof(OrderRdm).GetProperty(nameof(OrderRdm.Id))!.SetValue(order, orderId);
        typeof(OrderRdm).GetProperty(nameof(OrderRdm.CreatedAt))!.SetValue(order, createdAt);

        var mapper = new CreateOrderResponseMapper();

        // Act
        var result = mapper.MapToResponse(order);

        // Assert
        result.Should().NotBeNull();
        result.OrderNumber.Should().Be(orderId);
    }
}