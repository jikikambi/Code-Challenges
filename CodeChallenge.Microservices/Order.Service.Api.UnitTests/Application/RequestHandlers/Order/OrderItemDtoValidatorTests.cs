using AutoFixture;
using CodeChallenge.Shared.UnitTests;
using Order.Service.Api.Application.RequestHandlers.Validators;
using Order.Service.Shared.Model;

namespace Order.Service.Api.UnitTests.Application.RequestHandlers.Order;

public class OrderItemDtoValidatorTests : ValidatorTestsHelper<OrderItemDtoValidator, OrderItemDto>
{
    [Fact]
    public void Should_Fail_When_ProductId_Is_Empty()
    {
        var message = Fixture.Build<OrderItemDto>()
            .With(x => x.ProductId, string.Empty)
            .With(x => x.ProductName, "Gaming Laptop")
            .With(x => x.ProductAmount, 2)
            .With(x => x.ProductPrice, 1499.99M)
            .Create();

        ValidateRule(message, "Product ID is required.");
    }

    [Fact]
    public void Should_Fail_When_ProductName_Is_Empty()
    {
        var message = Fixture.Build<OrderItemDto>()
           .With(x => x.ProductId, "0f8fad5b-d9cb-469f-a165-70867728950e")
           .With(x => x.ProductName, string.Empty)
           .With(x => x.ProductAmount, 2)
           .With(x => x.ProductPrice, 1499.99M)
           .Create();

        ValidateRule(message, "Product name is required.");
    }

    [Fact]
    public void Should_Fail_When_ProductAmount_Is_Zero()
    {
        var message = Fixture.Build<OrderItemDto>()
          .With(x => x.ProductId, "0f8fad5b-d9cb-469f-a165-70867728950e")
          .With(x => x.ProductName, "Gaming Laptop")
          .With(x => x.ProductAmount, 0)
          .With(x => x.ProductPrice, 1499.99M)
          .Create();

        ValidateRule(message, "Product amount must be greater than 0.");
    }

    [Fact]
    public void Should_Fail_When_ProductPrice_Is_Zero()
    {
        var message = Fixture.Build<OrderItemDto>()
          .With(x => x.ProductId, "0f8fad5b-d9cb-469f-a165-70867728950e")
          .With(x => x.ProductName, "Gaming Laptop")
          .With(x => x.ProductAmount, 2)
          .With(x => x.ProductPrice, 0)
          .Create();

        ValidateRule(message, "Product price must be greater than 0.");
    }

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var message = Fixture.Build<OrderItemDto>()
            .With(x => x.ProductId, "0f8fad5b-d9cb-469f-a165-70867728950e")
            .With(x => x.ProductName, "Gaming Laptop")
            .With(x => x.ProductAmount, 2)
            .With(x => x.ProductPrice, 1499.99M)
            .Create();

        ValidateValidMessage(message);
    }
}
