using OrdersService.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Abstractions;

namespace OrdersService.Infrastructure.Repositories;

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
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        DbContext.Add(entity);
    }

    public void Update(T entity)
    {
        DbContext.Update(entity);
    }
}

