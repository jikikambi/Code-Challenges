using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.DomainLayer.Dtos;

[ExcludeFromCodeCoverage(Justification = "No Logic")]
public record OrderItemDto(
    string ProductId,
    string ProductName,
    int ProductAmount,
    decimal ProductPrice
);