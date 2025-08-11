using CodeChallenge.DomainLayer.Order.Events;
using CodeChallenge.DomainLayer.Order.Services;
using CodeChallenge.DomainLayer.Order.Specifications;
using CodeChallenge.InfrastructureLayer.Services;
using MediatR;
using Order.Service.Api.Application.Mappers;
using OrderRdm = CodeChallenge.DomainLayer.Order.Order;

namespace Order.Service.Api.Application.RequestHandlers.Commands.Create;

public class CreateOrderCommandRequestHandler(IOrderRepository orderRepository,
    IEventStoreRepository<OrderRdm> eventStoreRepository,
    IInventoryService inventoryService,
    ICreateOrderMapper mapper,
    IOrderItemMapper orderItemMapper
    )
    : IRequestHandler<CreateOrderCommandRequest, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var mappedOrderItems = orderItemMapper.MapToOrderItem(request.Input);

        var inStockSpec = new ProductInStockSpec(inventoryService);

        foreach (var item in mappedOrderItems)
        {
            if (!await inStockSpec.IsSatisfiedBy(mappedOrderItems))
                throw new ApplicationException("One or more products are out of stock.");
        }

        var order = mapper.MapToOrder(request.Input);
        await orderRepository.AddAsync(order);

        await eventStoreRepository.SaveAsync(order, cancellationToken);

        order.ClearUncommittedEvents();

        return order.Id;
    }
}