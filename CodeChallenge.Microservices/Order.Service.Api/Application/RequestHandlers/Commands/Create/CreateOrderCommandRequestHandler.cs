using CodeChallenge.InfrastructureLayer.Services;
using MediatR;
using Order.Service.Api.Application.Mappers;

namespace Order.Service.Api.Application.RequestHandlers.Commands.Create;

public class CreateOrderCommandRequestHandler(IOrderRepository orderRepository,
    ICreateOrderMapper mapper)
    : IRequestHandler<CreateOrderCommandRequest, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var order = mapper.MapToOrder(request.Input);
        await orderRepository.AddAsync(order);
        return order.Id;
    }
}