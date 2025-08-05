using CodeChallenge.ApplicationLayer;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Shared.Request;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public record OrderQueryRequest(Guid OrderNumber);