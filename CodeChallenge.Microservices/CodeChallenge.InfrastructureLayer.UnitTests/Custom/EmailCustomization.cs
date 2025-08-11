using AutoFixture;
using CodeChallenge.DomainLayer.ValueObjects;

namespace CodeChallenge.InfrastructureLayer.UnitTests.Custom;

public class EmailCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Email>(c =>
            c.FromFactory(() => new Email("customer@example.com")));
    }
}