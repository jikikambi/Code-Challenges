using AutoFixture;
using CodeChallenge.DomainLayer.Order;
using CodeChallenge.DomainLayer.ValueObjects;

namespace CodeChallenge.InfrastructureLayer.UnitTests.Custom;

public class AddressCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => new Address("123 Sample Street, 90402 Berlin"));
    }
}