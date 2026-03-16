using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product;

namespace CatalogService.Application.DeleteProduct;

internal sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        // Verify ownership
        if (product.OwnerUserId != request.OwnerUserId)
        {
            return Result.Failure(ProductErrors.Unauthorized);
        }

        // Use repository to delete instead of direct DbContext access
        _productRepository.Delete(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
