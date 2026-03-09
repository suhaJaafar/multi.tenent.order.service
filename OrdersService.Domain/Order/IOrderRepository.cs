using OrdersService.Domain.Order.Entities;

namespace OrdersService.Domain.Order;

public interface IOrderRepository
{
    Task<Entities.Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Entities.Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(Entities.Order order);
    void Update(Entities.Order order);
}

