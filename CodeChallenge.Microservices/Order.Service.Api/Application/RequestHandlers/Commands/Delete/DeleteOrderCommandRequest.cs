using CodeChallenge.ApplicationLayer;
using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Models;
using CodeChallenge.ApplicationLayer.Tracking.Models.Operation;
using MediatR;
using Order.Service.Shared.Request;
using System.Diagnostics.CodeAnalysis;

namespace Order.Service.Api.Application.RequestHandlers.Commands.Delete;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public class DeleteOrderCommandRequest(DeleteOrderRequest query)
    : TrackingRequestBase<DeleteOrderRequest, Unit>(
        CanonicalType.Order,
        new(ServiceId.OrderServiceApi, OrderServiceApiOperations.DeleteOrderCommandRequestHandler),
        query,
        query.OrderNumber.ToString());