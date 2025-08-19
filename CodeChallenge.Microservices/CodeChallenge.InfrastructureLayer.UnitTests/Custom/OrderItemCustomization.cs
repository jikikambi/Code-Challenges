using AutoFixture;
using CodeChallenge.DomainLayer.Order;

namespace CodeChallenge.InfrastructureLayer.UnitTests.Custom;

public class OrderItemCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => new OrderItem("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M));
    }
}