using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.DBContexts;
using OrdersService.Domain.Product;
using OrdersService.Domain.Product.Entities;

namespace OrdersService.Infrastructure.Repositories;

public class ProductReferenceRepository : IProductReferenceRepository
{
    private readonly OrdersContext _context;

    public ProductReferenceRepository(OrdersContext context)
    {
        _context = context;
    }

    public async Task<ProductReference?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductReferences
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
    }

    public async Task<List<ProductReference>> GetByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken = default)
    {
        return await _context.ProductReferences
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }

    public void Add(ProductReference productReference)
    {
        _context.ProductReferences.Add(productReference);
    }

    public void Update(ProductReference productReference)
    {
        _context.ProductReferences.Update(productReference);
    }

    public void Delete(ProductReference productReference)
    {
        _context.ProductReferences.Remove(productReference);
    }
}

