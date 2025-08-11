using AutoFixture;
using CodeChallenge.DomainLayer.ValueObjects;
using CodeChallenge.InfrastructureLayer.Services;
using FakeItEasy;
using FluentAssertions;
using Order.Service.Api.Application.Mappers;
using Order.Service.Api.Application.RequestHandlers.Queries;
using Order.Service.Shared.Request;
using Order.Service.Shared.Response;
using OrderRdm = CodeChallenge.DomainLayer.Order.Order;
using OrderItem = CodeChallenge.DomainLayer.Order.OrderItem;

namespace Order.Service.Api.UnitTests.Application.RequestHandlers.Order;

public class GetOrderByIdQueryRequestHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly IOrderRepository _repository = A.Fake<IOrderRepository>();
    private readonly IOrderResponseMapper _mapper = A.Fake<IOrderResponseMapper>();

    private readonly GetOrderByIdQueryRequestHandler _sut;

    public GetOrderByIdQueryRequestHandlerTests()
    {
        _sut = new GetOrderByIdQueryRequestHandler(_repository, _mapper);
    }

    [Fact]
    public async Task Should_Return_OrderResponse_When_Order_Exists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new OrderQueryRequest(orderId);
        var request = new GetOrderByIdQueryRequest(input);
        
        var address = new Address("123 Sample Street, 90402 Berlin");
        var email = new Email("customer@example.com");
        var creditCard = new CreditCard("1234-5678-9101-1121");
        var orderItem = new OrderItem("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99m);

        var order = OrderRdm.Create(address, email, creditCard, [orderItem]);

        var expectedResponse = _fixture.Create<OrderResponse>();

        A.CallTo(() => _repository.GetByIdAsync(orderId))
            .Returns(Task.FromResult<OrderRdm?>(order));

        A.CallTo(() => _mapper.MapToResponse(order))
            .Returns(expectedResponse);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenOrderIsNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new OrderQueryRequest(orderId);
        var request = new GetOrderByIdQueryRequest(input);

        A.CallTo(() => _repository.GetByIdAsync(orderId))
            .Returns(Task.FromResult<OrderRdm?>(null));

        // Act
       var result =  await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}