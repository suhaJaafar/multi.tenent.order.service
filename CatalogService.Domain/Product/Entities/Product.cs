using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product.Enums;
using CatalogService.Domain.Product.Events;

namespace CatalogService.Domain.Product.Entities;

public class Product : Entity
{
    private Product() : base(Guid.Empty)
    {
    }

    private Product(Guid id, string name, string description, decimal price, int stock,
        Guid ownerUserId, ProductCategory category) : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        OwnerUserId = ownerUserId;
        Category = category;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Guid OwnerUserId { get; set; }
    public ProductCategory Category { get; set; }
    public bool IsActive { get; set; } = true;

    public static Product Create(Guid id, string name, string description, decimal price, int stock,
        Guid ownerUserId, ProductCategory category)
    {
        var product = new Product(id, name, description, price, stock, ownerUserId, category);
        product.RaiseDomainEvent(new ProductCreatedDomainEvent(product.Id));
        return product;
    }
}