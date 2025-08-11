using CodeChallenge.ApplicationLayer;
using CodeChallenge.DomainLayer.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Shared.Response;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public record OrderResponse(
    Guid OrderNumber,
    string InvoiceAddress,
    string InvoiceEmail,
    string CreditCardNumber,
    List<OrderItemDto> Items,
    DateTime CreatedAt);