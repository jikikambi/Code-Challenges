using AutoFixture;
using FluentAssertions;
using FluentValidation;

namespace CodeChallenge.Shared.UnitTests;

public abstract class ValidatorTestsHelper<TValidator, TMessageType> where TValidator : AbstractValidator<TMessageType>, new()
{
    protected readonly Fixture Fixture = new();

    private readonly TValidator _sut = new();

    protected void ValidateRule(Action<TMessageType> setupFixture, string expectedErrorMessage)
    {
        // Arrange
        var message = Fixture.Create<TMessageType>();
        setupFixture(message);

        // Act & Assert
        ValidateRule(message, expectedErrorMessage);
    }

    protected void ValidateRule(TMessageType message, string expectedErrorMessage)
    {
        // Act
        var actual = _sut.Validate(message);

        // Assert
        actual.IsValid.Should().BeFalse();
        actual.Errors.Should().HaveCount(1);
        actual.Errors[0].ErrorMessage.Should().Be(expectedErrorMessage);
    }

    protected void ValidateValidMessage(TMessageType message)
    {
        // Act
        var actual = _sut.Validate(message);

        // Assert
        actual.IsValid.Should().BeTrue();
        actual.Errors.Should().BeEmpty();
    }
}