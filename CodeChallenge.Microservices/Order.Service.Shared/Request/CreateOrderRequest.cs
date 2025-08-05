using CodeChallenge.ApplicationLayer;
using Order.Service.Shared.Model;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Shared.Request;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public record CreateOrderRequest(
    List<OrderItemDto> Items,
    string InvoiceAddress,
    string InvoiceEmail,
    string CreditCardNumber);