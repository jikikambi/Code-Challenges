using CodeChallenge.ApplicationLayer;
using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Models;
using CodeChallenge.ApplicationLayer.Tracking.Models.Operation;
using Order.Service.Shared.Request;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Api.Application.RequestHandlers.Commands.Create;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public class CreateOrderCommandRequest(CreateOrderRequest query)
    : TrackingRequestBase<CreateOrderRequest, Guid>(
        CanonicalType.Order,
        new (ServiceId.OrderServiceApi, OrderServiceApiOperations.CreateOrderCommandRequestHandler),
        query,
        Guid.NewGuid().ToString());