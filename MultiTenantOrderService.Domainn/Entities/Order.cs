using MultiTenantOrderService.Domain.Enums;

namespace MultiTenantOrderService.Domain.Entities;
public class Order : BaseModel
{
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<Product> Products { get; } = [];
}