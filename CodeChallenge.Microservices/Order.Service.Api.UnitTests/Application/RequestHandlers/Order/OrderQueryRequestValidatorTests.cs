using AutoFixture;
using CodeChallenge.Shared.UnitTests;
using Order.Service.Api.Application.RequestHandlers.Validators;
using Order.Service.Shared.Request;

namespace Order.Service.Api.UnitTests.Application.RequestHandlers.Order;

public class OrderQueryRequestValidatorTests : ValidatorTestsHelper<OrderQueryRequestValidator, OrderQueryRequest>
{
    [Fact]
    public void Should_Fail_When_OrderNumber_Is_Empty()
    {
        var message = Fixture.Build<OrderQueryRequest>()
            .With(x => x.OrderNumber, Guid.Empty)
            .Create();

        ValidateRule(message, "OrderNumber is required.");
    }

    [Fact]
    public void Should_Pass_When_OrderNumber_Is_Valid()
    {
        var message = Fixture.Build<OrderQueryRequest>()
            .With(x => x.OrderNumber,new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") )
            .Create();

        ValidateValidMessage(message);
    }
}