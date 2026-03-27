using OrdersService.Domain.Product.Entities;

namespace OrdersService.Domain.Product;

public interface IProductReferenceRepository
{
    Task<ProductReference?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<List<ProductReference>> GetByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken = default);
    void Add(ProductReference productReference);
    void Update(ProductReference productReference);
    void Delete(ProductReference productReference);
}

