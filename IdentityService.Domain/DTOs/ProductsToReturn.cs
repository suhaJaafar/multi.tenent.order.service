
namespace IdentityService.Domain.DTOs;
public class ProductsToReturn
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public List<OrderInProductDto> Orders { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}