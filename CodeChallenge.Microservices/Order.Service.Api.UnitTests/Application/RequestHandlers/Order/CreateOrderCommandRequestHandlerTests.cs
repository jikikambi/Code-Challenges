using AutoFixture;
using CodeChallenge.DomainLayer.Order;
using CodeChallenge.DomainLayer.Order.Services;
using CodeChallenge.DomainLayer.ValueObjects;
using CodeChallenge.InfrastructureLayer.Services;
using FakeItEasy;
using FluentAssertions;
using Order.Service.Api.Application.Mappers;
using Order.Service.Api.Application.RequestHandlers.Commands.Create;
using Order.Service.Shared.Request;
using OrderRdm = CodeChallenge.DomainLayer.Order.Order;

namespace Order.Service.Api.UnitTests.Application.RequestHandlers.Order;

public class CreateOrderCommandRequestHandlerTests
{
    private readonly Fixture _fixture = new();

    private readonly IOrderRepository _orderRepository = A.Fake<IOrderRepository>();
    private readonly IEventStoreRepository<OrderRdm> _eventStoreRepository = A.Fake<IEventStoreRepository<OrderRdm>>();
    private readonly ICreateOrderMapper _orderMapper = A.Fake<ICreateOrderMapper>();
    private readonly IOrderItemMapper _orderItemMapper = A.Fake<IOrderItemMapper>();
    private readonly IInventoryService _inventoryService = A.Fake<IInventoryService>();

    private readonly CreateOrderCommandRequestHandler _sut;

    public CreateOrderCommandRequestHandlerTests()
    {
        _sut = new CreateOrderCommandRequestHandler(
            _orderRepository,
            _eventStoreRepository,
            _inventoryService,
            _orderMapper,
            _orderItemMapper);
    }

    [Fact]
    public async Task Should_Add_Order_And_Return_OrderId()
    {
        // Arrange
        var input = _fixture.Create<CreateOrderRequest>();
        var request = new CreateOrderCommandRequest(input);

        var orderItems = new List<OrderItem>
        {
            new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)
        };

        var order = OrderRdm.Create(
            new Address("123 Sample Street, 90402 Berlin"),
            new Email("customer@example.com"),
            new CreditCard("1234-5678-9101-1121"),
            orderItems
        );

        // Fakes
        A.CallTo(() => _orderItemMapper.MapToOrderItem(input)).Returns(orderItems);

        // Make sure inventory check returns true
        A.CallTo(() => _inventoryService.IsInStockAsync(orderItems[0].ProductId, orderItems[0].ProductAmount, A<CancellationToken>._))
            .Returns(true);

        A.CallTo(() => _orderMapper.MapToOrder(input)).Returns(order);
        A.CallTo(() => _orderRepository.AddAsync(order)).Returns(order.Id);
        A.CallTo(() => _eventStoreRepository.SaveAsync(order, A<CancellationToken>._)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        A.CallTo(() => _orderItemMapper.MapToOrderItem(input)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _inventoryService.IsInStockAsync(orderItems[0].ProductId, orderItems[0].ProductAmount, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _orderMapper.MapToOrder(input)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _orderRepository.AddAsync(order)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _eventStoreRepository.SaveAsync(order, A<CancellationToken>._)).MustHaveHappenedOnceExactly();

        result.Should().Be(order.Id);
    }
}