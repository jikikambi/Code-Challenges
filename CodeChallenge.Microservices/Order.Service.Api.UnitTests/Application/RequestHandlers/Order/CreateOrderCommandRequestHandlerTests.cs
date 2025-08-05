using AutoFixture;
using CodeChallenge.DomainLayer.ValueObjects;
using CodeChallenge.InfrastructureLayer.Services;
using FakeItEasy;
using FluentAssertions;
using Order.Service.Api.Application.Mappers;
using Order.Service.Api.Application.RequestHandlers.Commands.Create;
using Order.Service.Shared.Request;
using OrderRdm = CodeChallenge.DomainLayer.Entities.Order;

namespace Order.Service.Api.UnitTests.Application.RequestHandlers.Order;

public class CreateOrderCommandRequestHandlerTests
{
    private readonly IOrderRepository _orderRepository = A.Fake<IOrderRepository>();
    private readonly ICreateOrderMapper _mapper = A.Fake<ICreateOrderMapper>();
    private readonly Fixture _fixture = new();
    private readonly CreateOrderCommandRequestHandler _sut;

    public CreateOrderCommandRequestHandlerTests()
    {
        _sut = new CreateOrderCommandRequestHandler(_orderRepository, _mapper);
    }

    [Fact]
    public async Task Should_Add_Order_And_Return_OrderId()
    {
        // Arrange
        var input = _fixture.Create<CreateOrderRequest>();
        var request = new CreateOrderCommandRequest(input);

        var order = new OrderRdm(
            new Address("123 Sample Street, 90402 Berlin"),
            new Email("customer@example.com"),
            new CreditCard("1234-5678-9101-1121"),
            [
                new("0f8fad5b-d9cb-469f-a165-70867728950e", "Gaming Laptop", 2, 1499.99M)
            ]);

        A.CallTo(() => _mapper.MapToOrder(input)).Returns(order);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        A.CallTo(() => _orderRepository.AddAsync(order)).MustHaveHappenedOnceExactly();
        result.Should().Be(order.Id);
    }
}