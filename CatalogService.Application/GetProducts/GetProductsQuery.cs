using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Domain.Product.FiltersParams;

namespace CatalogService.Application.GetProducts;

public record GetProductsQuery(ProductsParams Parameters) : IQuery<ProductsListResponse>;

