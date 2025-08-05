using CodeChallenge.DomainLayer.Exceptions;
using FluentAssertions;
using Order.Service.Api.Application.Mappers;
using Order.Service.Shared.Model;
using Order.Service.Shared.Request;

namespace Order.Service.Api.UnitTests.Application.Mappers;

public class CreateOrderMapperTests
{
    private readonly CreateOrderMapper _sut = new();

    [Fact]
    public void Should_Map_CreateOrderRequest_To_Order_Correctly()
    {
        // Arrange
        var items = new List<OrderItemDto>
    {
        new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M),
        new("5e8fad5b-d9cb-469f-a165-70867728950d", "Gaming Controller", 1, 29.99M)
    };

        var request = new CreateOrderRequest(
            items,
            "123 Sample Street, 90402 Berlin",
            "customer@example.com",
            "1234-5678-9101-1121"
        );

        // Act
        var result = _sut.MapToOrder(request);

        // Assert
        result.Should().NotBeNull();
        result.InvoiceAddress.Value.Should().Be("123 Sample Street, 90402 Berlin");
        result.InvoiceEmail.Value.Should().Be("customer@example.com");
        result.CreditCard.Value.Should().Be("1234-5678-9101-1121");

        result.Items.Should().HaveCount(2);

        var itemList = result.Items.ToList();

        itemList[0].ProductId.Should().Be("0f8fad5b-d9cb-469f-a165-70867728950e");
        itemList[0].ProductName.Should().Be("Gaming Laptop");
        itemList[0].ProductAmount.Should().Be(2);
        itemList[0].ProductPrice.Should().Be(1499.99M);
    }


    [Fact]
    public void Should_Throw_When_Email_Is_Invalid()
    {
        var request = new CreateOrderRequest(
            [
                new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)
            ],
            "123 Sample Street, 90402 Berlin",
            "not-an-email",
            "1234-5678-9101-1121"
        );

        // Act
        Action act = () => _sut.MapToOrder(request);

        // Assert
        act.Should().Throw<InvalidEmailException>()
           .WithMessage("Invalid email address: not-an-email");
    }

    //[Fact]
    //public void Should_Throw_When_CreditCard_Is_Invalid()
    //{
    //    var request = new CreateOrderRequest(
    //        [
    //            new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)
    //        ],
    //        "123 Sample Street, 90402 Berlin",
    //        "customer@example.com",
    //        "invalid-card"
    //    );

    //    // Act
    //    Action act = () => _sut.MapToOrder(request);

    //    // Assert
    //    act.Should().Throw<InvalidCreditCardException>()
    //       .WithMessage("Credit card number must be in format ####-####-####-####.");
    //}

    [Fact]
    public void Should_Throw_When_Items_Are_Empty()
    {
        var request = new CreateOrderRequest(
            [],
            "123 Sample Street, 90402 Berlin",
            "customer@example.com",
            "1234-5678-9101-1121"
        );

        // Act
        Action act = () => _sut.MapToOrder(request);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("Order must contain at least one item.");
    }
}