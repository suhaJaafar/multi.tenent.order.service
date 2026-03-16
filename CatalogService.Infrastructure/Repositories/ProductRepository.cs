using Microsoft.EntityFrameworkCore;
using CatalogService.Domain.Product;
using CatalogService.Domain.Product.Entities;

namespace CatalogService.Infrastructure.Repositories;

internal sealed class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(DbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<List<Product>> GetByOwnerUserIdAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Product>()
            .Where(p => p.OwnerUserId == ownerUserId)
            .ToListAsync(cancellationToken);
    }

    public void Delete(Product product)
    {
        DbContext.Set<Product>().Remove(product);
    }
}
