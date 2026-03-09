using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Order;
using OrdersService.Domain.Order.Entities;

namespace OrdersService.Infrastructure.Repositories;

internal sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(DbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<List<Order>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Order>()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreateAt)
            .ToListAsync(cancellationToken);
    }
}

