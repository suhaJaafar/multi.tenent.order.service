using CatalogService.Domain.Product.Enums;
using CatalogService.Domain.SharedParams;

namespace CatalogService.Domain.Product.FiltersParams;

public class ProductsParams : BaseParams
{
    public ProductCategory? Category { get; set; }
    public Guid? OwnerUserId { get; set; }
    public bool? IsActive { get; set; }
}

