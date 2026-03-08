namespace MultiTenantOrderService.Domain.Entities;
public class Product : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int? Stock { get; set; }
    public string? ImageUrl { get; set; }
    public List<Order> Orders { get; } = [];
    public List<User> FavoritedByUsers { get; } = [];  // Users who favorited this product
}