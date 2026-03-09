using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Domain.Product.Enums;

namespace CatalogService.Application.CreateProduct;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    ProductCategory Category);

