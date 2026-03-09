namespace CatalogService.Application.GetProduct;

public sealed class ProductResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public Guid OwnerUserId { get; init; }
    public string Category { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreateAt { get; init; }
    public DateTime? UpdateAt { get; init; }
}

