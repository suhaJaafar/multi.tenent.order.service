using MultiTenantOrderService.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using MultiTenantOrderService.Domain.Abstractions;

namespace MultiTenantOrderService.Infrastructure.Repositories;

internal abstract class Repository<T>
    where T : Entity
{
    protected readonly DbContext DbContext;

    protected Repository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<T>()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        DbContext.Add(entity);
    }
}