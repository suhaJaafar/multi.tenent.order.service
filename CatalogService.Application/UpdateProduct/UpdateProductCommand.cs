using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.GetProduct;
using CatalogService.Domain.Product.Enums;

namespace CatalogService.Application.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid OwnerUserId,
    ProductCategory Category,
    bool IsActive) : ICommand<ProductResponse>;

