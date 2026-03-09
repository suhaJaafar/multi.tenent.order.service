namespace CatalogService.Domain.Product.DTOs;

public class ProductToReturnDto
{
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Guid OwnerUserId { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

