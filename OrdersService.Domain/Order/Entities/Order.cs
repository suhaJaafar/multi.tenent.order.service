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
        Notes = null;
    }

    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? Notes { get; set; }
    
    public Guid UserId { get; private set; }
    
    // Order items collection
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public static Order Create(Guid id, Guid userId, string? notes = null)
    {
        var order = new Order(id, userId, 0, OrderStatus.Pending) // Start with 0, will be calculated from items
        {
            Notes = notes
        };
        
        // Domain event will be raised after order items are added
        
        return order;
    }

    /// <summary>
    /// Add an item to the order
    /// </summary>
    public void AddOrderItem(OrderItem item)
    {
        _orderItems.Add(item);
        RecalculateTotalAmount();
    }

    /// <summary>
    /// Remove an item from the order
    /// </summary>
    public void RemoveOrderItem(Guid itemId)
    {
        var item = _orderItems.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _orderItems.Remove(item);
            RecalculateTotalAmount();
        }
    }

    /// <summary>
    /// Recalculate total amount from order items
    /// </summary>
    private void RecalculateTotalAmount()
    {
        TotalAmount = _orderItems.Sum(item => item.Subtotal);
        UpdateAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Confirm the order and raise domain event
    /// </summary>
    public void Confirm()
    {
        if (!_orderItems.Any())
            throw new InvalidOperationException("Cannot confirm order without items");

        RaiseDomainEvent(new OrderCreatedDomainEvent(Id, UserId, TotalAmount));
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

