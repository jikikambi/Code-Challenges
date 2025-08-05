using CodeChallenge.InfrastructureLayer.Services;
using MediatR;

namespace Order.Service.Api.Application.RequestHandlers.Commands.Delete;

public class DeleteOrderCommandRequestHandler(IOrderRepository orderRepository)
    : IRequestHandler<DeleteOrderCommandRequest, Unit>
{
    public async Task<Unit> Handle(DeleteOrderCommandRequest request, CancellationToken cancellationToken)
    {
        await orderRepository.DeleteAsync(request.Input.OrderNumber);
        return Unit.Value;
    }
}