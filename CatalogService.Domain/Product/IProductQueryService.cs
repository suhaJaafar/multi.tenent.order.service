using CatalogService.Domain.Product.Entities;
using CatalogService.Domain.Product.FiltersParams;
using CatalogService.Domain.SharedParams;

namespace CatalogService.Domain.Product;

/// <summary>
/// Read-only query interface for Product queries
/// This allows Application layer to query products without depending on Infrastructure
/// </summary>
public interface IProductQueryService
{
    Task<(List<Entities.Product> Products, int TotalCount)> GetProductsAsync(
        ProductsParams parameters,
        CancellationToken cancellationToken = default);
}

