using CodeChallenge.InfrastructureLayer.Services;
using FakeItEasy;
using FluentAssertions;
using MediatR;
using Order.Service.Api.Application.RequestHandlers.Commands.Delete;
using Order.Service.Shared.Request;

namespace Order.Service.Api.UnitTests.Application.RequestHandlers.Order;

public class DeleteOrderCommandRequestHandlerTests
{
    private readonly IOrderRepository _repository = A.Fake<IOrderRepository>();

    private readonly DeleteOrderCommandRequestHandler _sut;

    public DeleteOrderCommandRequestHandlerTests()
    {
        _sut = new DeleteOrderCommandRequestHandler(_repository);
    }

    [Fact]
    public async Task Handle_Should_Return_Unit_When_Order_Is_Deleted()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new DeleteOrderRequest(orderId);
        var request = new DeleteOrderCommandRequest(input);

        A.CallTo(() => _repository.DeleteAsync(orderId))
        .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        result.Should().Be(Unit.Value);
        A.CallTo(() => _repository.DeleteAsync(orderId)).MustHaveHappenedOnceExactly();
    }
}