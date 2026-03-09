namespace CatalogService.Domain.Product;

public interface IProductRepository
{
    Task<Entities.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Entities.Product>> GetByOwnerUserIdAsync(Guid ownerUserId, CancellationToken cancellationToken = default);
    void Add(Entities.Product product);
}