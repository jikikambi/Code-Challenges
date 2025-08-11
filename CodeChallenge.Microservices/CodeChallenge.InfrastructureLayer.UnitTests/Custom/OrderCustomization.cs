using AutoFixture;
using CodeChallenge.DomainLayer.Order;
using CodeChallenge.DomainLayer.ValueObjects;

namespace CodeChallenge.InfrastructureLayer.UnitTests.Custom;

public class OrderCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Order>(composer =>
            composer.FromFactory(() =>
                Order.Create(
                    new Address("123 Sample Street, 90402 Berlin"),
                    new Email("customer@example.com"),
                    new CreditCard("1234-5678-9101-1121"),
                    [new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)]
                )));
    }
}