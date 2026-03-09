using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Order.Enums;
using OrdersService.Domain.Order.Events;

namespace OrdersService.Domain.Order.Entities;

public class Order : Entity
{
    // Parameterless constructor for EF Core
    private Order() : base(Guid.Empty)
    {
    }
    
    private Order(Guid id, Guid userId, decimal totalAmount, OrderStatus status)
        : base(id)
    {
        UserId = userId;
        TotalAmount = totalAmount;
        Status = status;
    }

    public Guid UserId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? Notes { get; set; }

    public static Order Create(Guid id, Guid userId, decimal totalAmount, string? notes = null)
    {
        var order = new Order(id, userId, totalAmount, OrderStatus.Pending)
        {
            Notes = notes
        };
        
        order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.Id, order.UserId, order.TotalAmount));
        
        return order;
    }

    public void UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
        UpdateAt = DateTime.UtcNow;
    }

    public void UpdateAmount(decimal newAmount)
    {
        TotalAmount = newAmount;
        UpdateAt = DateTime.UtcNow;
    }
}

