using CodeChallenge.ApplicationLayer;
using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Models;
using CodeChallenge.ApplicationLayer.Tracking.Models.Operation;
using Order.Service.Shared.Response;
using Order.Service.Shared.Request;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Api.Application.RequestHandlers.Queries;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public class GetOrderByIdQueryRequest(OrderQueryRequest query)
    : TrackingRequestBase<OrderQueryRequest, OrderResponse>(
        CanonicalType.Order,
        new (ServiceId.OrderServiceApi, OrderServiceApiOperations.GetOrderByIdQueryRequestHandler),
        query,
        query.OrderNumber.ToString());