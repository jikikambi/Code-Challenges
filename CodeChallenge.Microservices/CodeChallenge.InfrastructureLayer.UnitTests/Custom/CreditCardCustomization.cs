using AutoFixture;
using CodeChallenge.DomainLayer.ValueObjects;

namespace CodeChallenge.InfrastructureLayer.UnitTests.Custom;

public class CreditCardCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => new CreditCard("1234-5678-9101-1121")); 
    }
}