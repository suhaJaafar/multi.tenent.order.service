using OrdersService.Domain.Abstractions;

namespace OrdersService.Domain.Product.Entities;

/// <summary>
/// Local product reference data maintained by consuming events from Catalog Service.
/// This is NOT the full product model - only what OrdersService needs.
/// </summary>
public class ProductReference : Entity
{
    private ProductReference() : base(Guid.Empty)
    {
    }

    private ProductReference(
        Guid productId,
        string sku,
        string name,
        decimal currentPrice,
        string category,
        bool isActive,
        Guid ownerUserId) : base(productId)
    {
        SKU = sku;
        Name = name;
        CurrentPrice = currentPrice;
        Category = category;
        IsActive = isActive;
        OwnerUserId = ownerUserId;
        LastSyncedAt = DateTime.UtcNow;
    }

    public string SKU { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public decimal CurrentPrice { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public Guid OwnerUserId { get; private set; }
    public DateTime LastSyncedAt { get; private set; }

    /// <summary>
    /// Factory method to create a new product reference from integration event
    /// </summary>
    public static ProductReference Create(
        Guid productId,
        string sku,
        string name,
        decimal price,
        string category,
        bool isActive,
        Guid ownerUserId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        if (price < 0)
            throw new ArgumentException("Product price cannot be negative", nameof(price));

        return new ProductReference(productId, sku, name, price, category, isActive, ownerUserId);
    }

    /// <summary>
    /// Update product reference from integration event
    /// </summary>
    public void Update(string name, decimal price, string category, bool isActive)
    {
        Name = name;
        CurrentPrice = price;
        Category = category;
        IsActive = isActive;
        LastSyncedAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Update only price (from ProductPriceChanged event)
    /// </summary>
    public void UpdatePrice(decimal newPrice)
    {
        CurrentPrice = newPrice;
        LastSyncedAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Mark product as archived/inactive
    /// </summary>
    public void Archive()
    {
        IsActive = false;
        LastSyncedAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if product can be ordered
    /// </summary>
    public bool IsOrderable() => IsActive;
}

