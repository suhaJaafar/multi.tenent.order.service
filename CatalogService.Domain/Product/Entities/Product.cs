using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product.Enums;
using CatalogService.Domain.Product.Events;

namespace CatalogService.Domain.Product.Entities;

public class Product : Entity
{
    // Parameterless constructor for EF Core
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
        product.RaiseDomainEvent(new ProductCreatedDomainEvent(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.OwnerUserId,
            product.Category,
            product.IsActive));
        return product;
    }

    public void Update(string name, string description, decimal price, int stock, ProductCategory category)
    {
        var oldPrice = Price;
        
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        Category = category;
        UpdateAt = DateTime.UtcNow;

        // If price changed, raise a price change event
        if (oldPrice != price)
        {
            RaiseDomainEvent(new ProductPriceChangedDomainEvent(Id, oldPrice, price));
        }
        
        RaiseDomainEvent(new ProductUpdatedDomainEvent(Id, name, description, price, stock, category));
    }

    public void Archive()
    {
        IsActive = false;
        UpdateAt = DateTime.UtcNow;
        RaiseDomainEvent(new ProductArchivedDomainEvent(Id));
    }
}