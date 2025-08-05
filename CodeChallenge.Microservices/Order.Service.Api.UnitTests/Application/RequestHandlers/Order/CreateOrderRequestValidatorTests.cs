using AutoFixture;
using CodeChallenge.Shared.UnitTests;
using Order.Service.Api.Application.RequestHandlers.Validators;
using Order.Service.Shared.Request;

namespace Order.Service.Api.UnitTests.Application.RequestHandlers.Order;

public class CreateOrderRequestValidatorTests
    : ValidatorTestsHelper<CreateOrderRequestValidator, CreateOrderRequest>
{
    [Fact]
    public void Should_Fail_When_InvoiceAddress_Is_Empty()
    {
        var message = Fixture.Build<CreateOrderRequest>()
            .With(x => x.InvoiceAddress, string.Empty)
            .With(x => x.InvoiceEmail, "customer@example.com")
            .With(x => x.CreditCardNumber, "1234-5678-9101-1121")
            .With(x => x.Items, [new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)])
            .Create();

        ValidateRule(message, "Invoice address is required.");
    }

    [Fact]
    public void Should_Fail_When_InvoiceEmail_Is_Empty()
    {
        var message = Fixture.Build<CreateOrderRequest>()
            .With(x => x.InvoiceEmail, string.Empty)
            .With(x => x.InvoiceAddress, "123 Sample Street, 90402 Berlin")
            .With(x => x.CreditCardNumber, "1234-5678-9101-1121")
            .With(x => x.Items, [new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)])
            .Create();

        ValidateRule(message, "Email address is required.");
    }

    [Fact]
    public void Should_Fail_When_InvoiceEmail_Is_Invalid()
    {
        var message = Fixture.Build<CreateOrderRequest>()
            .With(x => x.InvoiceEmail, "not-an-email")
            .With(x => x.InvoiceAddress, "123 Sample Street, 90402 Berlin")
            .With(x => x.CreditCardNumber, "1234-5678-9101-1121")
            .With(x => x.Items, [new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)])
            .Create();

        ValidateRule(message, "A valid email address is required.");
    }

    [Fact]
    public void Should_Fail_When_CreditCardNumber_Is_Empty()
    {
        var message = Fixture.Build<CreateOrderRequest>()
            .With(x => x.CreditCardNumber, string.Empty)
            .With(x => x.InvoiceAddress, "123 Sample Street, 90402 Berlin")
            .With(x => x.InvoiceEmail, "customer@example.com")
            .With(x => x.Items, [new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)])
            .Create();

        ValidateRule(message, "Credit card number is required.");
    }

    [Fact]
    public void Should_Fail_When_CreditCardNumber_Is_Invalid()
    {
        var message = Fixture.Build<CreateOrderRequest>()
            .With(x => x.CreditCardNumber, "1234567890123456")
            .With(x => x.InvoiceAddress, "123 Sample Street, 90402 Berlin")
            .With(x => x.InvoiceEmail, "customer@example.com")
            .With(x => x.Items, [new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)])
            .Create();

        ValidateRule(message, "Credit card number must be in format ####-####-####-####.");
    }

    [Fact]
    public void Should_Fail_When_Items_Is_Empty()
    {
        var message = Fixture.Build<CreateOrderRequest>()
        .With(x => x.Items, [])
        .With(x => x.InvoiceAddress, "123 Sample Street, 90402 Berlin")
        .With(x => x.InvoiceEmail, "customer@example.com")
        .With(x => x.CreditCardNumber, "1234-5678-9101-1121")
        .Create();

        ValidateRule(message, "At least one order item is required.");
    }

    [Fact]
    public void Should_Pass_With_Valid_Request()
    {
        var message = Fixture.Build<CreateOrderRequest>()
            .With(x => x.InvoiceEmail, "customer@example.com")
            .With(x => x.InvoiceAddress, "123 Sample Street, 90402 Berlin")
            .With(x => x.CreditCardNumber, "1234-5678-9101-1121")
            .With(x => x.Items,
            [
                new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)
            ])
            .Create();

        ValidateValidMessage(message);
    }
}