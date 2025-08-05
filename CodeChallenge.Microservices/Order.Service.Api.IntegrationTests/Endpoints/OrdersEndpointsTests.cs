using CodeChallenge.Shared.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Order.Service.Shared.Request;
using Polly;
using Polly.Retry;
using Refit;
using Order.Service.Shared.Response;
using Order.Service.Api.Client;

namespace Order.Service.Api.IntegrationTests.Endpoints;

public class OrdersEndpointsTests(StartupFixture startupFixture) : IClassFixture<StartupFixture>
{
    private readonly IOrderServiceApiClient _orderServiceApiClient = startupFixture.ServiceProvider.GetRequiredService<IOrderServiceApiClient>();
    private readonly CreateOrderRequest _createOrderRequest = TestDataHelper.GetTestDataFromDisk<CreateOrderRequest>(nameof(OrdersEndpointsTests), "OrdersEndpointsTests.json");

    [Fact]
    public async Task CreateOrderAsync_WithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var resiliencePipeline = CreateResiliencePipeline();
        var createOrderRequest = _createOrderRequest;

        ApiResponse<CreateOrderResponse>? response = null;
        Guid orderNumber = Guid.Empty;

        try
        {
            // Act
            await resiliencePipeline.ExecuteAsync(async (token) =>
            {
                response = await _orderServiceApiClient.CreateOrderAsync(createOrderRequest);
            });

            // Assert
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Should().NotBeNull();
            response.Content.OrderNumber.Should().NotBe(Guid.Empty);

            orderNumber = response.Content.OrderNumber;
        }
        finally
        {
            if (orderNumber != Guid.Empty)
            {
                await _orderServiceApiClient.DeleteOrderAsync(orderNumber);
            }
        }
    }

    [Fact]
    public async Task GetOrderAsync_WithExistingOrderNumber_ReturnsOrderDetails()
    {
        // Arrange
        var resiliencePipeline = CreateResiliencePipeline();
        var createOrderRequest = _createOrderRequest;

        var createResponse = await _orderServiceApiClient.CreateOrderAsync(createOrderRequest);
        var orderNumber = createResponse.Content.OrderNumber;

        ApiResponse<OrderDetailResponse>? response = null;

        try
        {
            await resiliencePipeline.ExecuteAsync(async (token) =>
            {
                // Act
                response = await _orderServiceApiClient.GetOrderAsync(orderNumber, token);

                // Assert
                response.Should().NotBeNull();
                response.IsSuccessStatusCode.Should().BeTrue();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Content.Should().NotBeNull();
                response.Content.OrderNumber.Should().Be(orderNumber);
            });
        }
        finally
        {
            await _orderServiceApiClient.DeleteOrderAsync(orderNumber);
        }
    }

    [Fact]
    public async Task GetOrderAsync_WithNonExistentOrderNumber_ReturnsNotFound()
    {
        // Arrange
        var nonExistentOrderNumber = Guid.NewGuid();

        // Act
        var getResponse = await _orderServiceApiClient.GetOrderAsync(nonExistentOrderNumber);

        // Assert
        getResponse.Should().NotBeNull();
        getResponse.IsSuccessStatusCode.Should().BeFalse();
        getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    private static ResiliencePipeline CreateResiliencePipeline() 
        => new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                MaxRetryAttempts = 8,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential
            })
            .Build();
}