using OrdersService.Domain.Abstractions;

namespace OrdersService.Domain.Order.Entities;

/// <summary>
/// Represents a line item in an order with immutable product snapshot.
/// This preserves the product details at the time of order creation.
/// </summary>
public class OrderItem : Entity
{
    private OrderItem() : base(Guid.Empty)
    {
    }

    private OrderItem(
        Guid id,
        Guid orderId,
        Guid productId,
        string sku,
        string productName,
        decimal unitPrice,
        int quantity) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        SKU = sku;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Subtotal = unitPrice * quantity;
    }

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string SKU { get; private set; } = string.Empty;
    public string ProductName { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal Subtotal { get; private set; }

    public Order Order { get; set; } = null!;

    /// <summary>
    /// Factory method to create an order item with product snapshot
    /// </summary>
    public static OrderItem Create(
        Guid id,
        Guid orderId,
        Guid productId,
        string sku,
        string productName,
        decimal unitPrice,
        int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be empty", nameof(productName));

        return new OrderItem(id, orderId, productId, sku, productName, unitPrice, quantity);
    }

    /// <summary>
    /// Update quantity (only during order creation/modification, before confirmation)
    /// </summary>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));

        Quantity = newQuantity;
        Subtotal = UnitPrice * newQuantity;
        UpdateAt = DateTime.UtcNow;
    }
}

