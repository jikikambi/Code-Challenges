using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.ApplicationLayer.Tracking.Models.Operation;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public static class OrderServiceApiOperations
{
    public const string CreateOrderCommandRequestHandler = nameof(CreateOrderCommandRequestHandler);

    public static string GetOrderByIdQueryRequestHandler = nameof(GetOrderByIdQueryRequestHandler);

    public static string DeleteOrderCommandRequestHandler = nameof(DeleteOrderCommandRequestHandler);
}