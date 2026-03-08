using MultiTenantOrderService.Domain.Abstractions;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Entities;

namespace MultiTenantOrderService.Domain.Entities;
public class Order : Entity
{
    private Order() : base(Guid.Empty)
    {
    }
    
    private Order(Guid id, int quantity, decimal amount, OrderStatus orderStatus) : base(id)
    {
        Quantity = quantity;
        Amount = amount;
        OrderStatus = orderStatus;
    }
    
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public Guid UserId { get; set; }
    public Identity.Entities.User? User { get; set; }
    public List<Product> Products { get; } = [];
}