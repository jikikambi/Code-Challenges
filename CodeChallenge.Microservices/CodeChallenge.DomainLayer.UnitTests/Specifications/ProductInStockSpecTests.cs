using AutoFixture;
using CodeChallenge.DomainLayer.Order;
using CodeChallenge.DomainLayer.Order.Services;
using CodeChallenge.DomainLayer.Order.Specifications;
using FakeItEasy;

namespace CodeChallenge.DomainLayer.UnitTests.Specifications;

public class ProductInStockSpecTests
{
    private readonly Fixture _fixture;
    private readonly IInventoryService _inventoryService;

    public ProductInStockSpecTests()
    {
        _fixture = new Fixture();
        _inventoryService = A.Fake<IInventoryService>();
    }

    [Theory]
    [MemberData(nameof(GetOrderItemsData))]
    public async Task IsSatisfiedBy_ShouldReturnExpectedResult(List<OrderItem> items, bool expected)
    {
        // Arrange: mock inventory behavior based on test case
        foreach (var item in items)
        {
            A.CallTo(() => _inventoryService.IsInStockAsync(item.ProductId, item.ProductAmount, A<CancellationToken>._))
                .Returns(expected); // if expected == false, at least one will be false
        }

        var spec = new ProductInStockSpec(_inventoryService);

        // Act
        var result = await spec.IsSatisfiedBy(items);

        // Assert
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> GetOrderItemsData()
    {
        // Case 1: All in stock
        yield return new object[]
        {
            new List<OrderItem>
            {
                new(Guid.NewGuid().ToString(), "Product A", 5, 10m),
                new(Guid.NewGuid().ToString(), "Product B", 3, 15m)
            },
            true
        };

        // Case 2: One product out of stock (simulate by zero amount)
        yield return new object[]
        {
            new List<OrderItem>
            {
                new(Guid.NewGuid().ToString(), "Product C", 0, 5m),
                new(Guid.NewGuid().ToString(), "Product D", 2, 7m)
            },
            false
        };

        // Case 3: Edge case — minimal amount, but valid
        yield return new object[]
        {
            new List<OrderItem>
            {
                new(Guid.NewGuid().ToString(), "Product E", 1, 9m)
            },
            true
        };
    }
}