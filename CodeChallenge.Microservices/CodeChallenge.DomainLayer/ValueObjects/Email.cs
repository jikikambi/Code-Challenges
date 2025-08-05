using CodeChallenge.DomainLayer.Exceptions;

namespace CodeChallenge.DomainLayer.ValueObjects;

public record Email
{
    public string Value { get; init; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            throw new InvalidEmailException(value);
        Value = value;
    }

    public override string ToString() => Value;
}