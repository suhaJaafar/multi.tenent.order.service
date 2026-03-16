using Microsoft.EntityFrameworkCore;
using CatalogService.Domain.Product;
using CatalogService.Domain.Product.Entities;
using CatalogService.Domain.Product.FiltersParams;
using CatalogService.Domain.SharedParams;

namespace CatalogService.Infrastructure.Repositories;

internal sealed class ProductQueryService : IProductQueryService
{
    private readonly DbContext _dbContext;

    public ProductQueryService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(List<Product> Products, int TotalCount)> GetProductsAsync(
        ProductsParams parameters,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Product>()
            .OrderByDescending(v => v.CreateAt)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(parameters.SearchTerm))
        {
            var searchTerm = parameters.SearchTerm.ToLower().Replace(" ", "");
            query = query.Where(x =>
                x.Name.ToLower().Replace(" ", "").Contains(searchTerm) ||
                x.Description.ToLower().Replace(" ", "").Contains(searchTerm));
        }

        // Apply filters
        if (parameters.Id.HasValue)
            query = query.Where(x => x.Id == parameters.Id);

        if (parameters.Category.HasValue)
            query = query.Where(x => x.Category == parameters.Category);

        if (parameters.OwnerUserId.HasValue)
            query = query.Where(x => x.OwnerUserId == parameters.OwnerUserId);

        if (parameters.IsActive.HasValue)
            query = query.Where(x => x.IsActive == parameters.IsActive);

        // Get total count
        var count = await query.CountAsync(cancellationToken);

        // Apply pagination
        var products = await query
            .Skip(parameters.Skip)
            .Take(parameters.Take)
            .ToListAsync(cancellationToken);

        return (products, count);
    }
}

