using IdentityService.Domain.Abstractions;
using IdentityService.Domain.Identity.Entities;

namespace IdentityService.Domain.Entities;
public class Product : Entity
{
    private Product() : base(Guid.Empty)
    {
    }
    
    private Product(Guid id, string name, string description, decimal price, int? stock, string? imageUrl) : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        ImageUrl = imageUrl;
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int? Stock { get; set; }
    public string? ImageUrl { get; set; }
    public List<Order> Orders { get; } = [];
    public List<Identity.Entities.User> FavoritedByUsers { get; } = [];  // Users who favorited this product
}