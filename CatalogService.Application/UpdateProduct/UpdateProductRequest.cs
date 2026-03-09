using CatalogService.Domain.Product.Enums;

namespace CatalogService.Application.UpdateProduct;

public record UpdateProductRequest(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    ProductCategory Category,
    bool IsActive);

