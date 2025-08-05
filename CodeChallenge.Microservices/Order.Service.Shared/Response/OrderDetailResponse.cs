using Order.Service.Shared.Model;

namespace Order.Service.Shared.Response;

public class OrderDetailResponse
{
    public Guid OrderNumber { get; set; }
    public List<OrderItemDto> Items { get; set; } = [];
    public string Status { get; set; } = string.Empty;
}