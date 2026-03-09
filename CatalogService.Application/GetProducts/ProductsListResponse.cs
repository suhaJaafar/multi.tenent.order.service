using CatalogService.Domain.Product.DTOs;

namespace CatalogService.Application.GetProducts;

public sealed class ProductsListResponse
{
    public List<ProductToReturnDto> Products { get; init; } = new();
    public int TotalCount { get; init; }
}

