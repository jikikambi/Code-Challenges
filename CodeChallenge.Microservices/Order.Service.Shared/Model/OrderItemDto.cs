using CodeChallenge.ApplicationLayer;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Shared.Model;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public record OrderItemDto(
    string ProductId,
    string ProductName,
    int ProductAmount,
    decimal ProductPrice
);