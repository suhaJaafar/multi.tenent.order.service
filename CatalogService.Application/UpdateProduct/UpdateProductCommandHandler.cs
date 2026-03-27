using AutoMapper;
using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.GetProduct;
using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product;

namespace CatalogService.Application.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductResponse>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        // Get existing product
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            return Result.Failure<ProductResponse>(ProductErrors.NotFound);
        }

        // Verify ownership - only the product owner can update
        if (product.OwnerUserId != request.OwnerUserId)
        {
            return Result.Failure<ProductResponse>(ProductErrors.Unauthorized);
        }

        // Update product using domain method (will raise domain events)
        product.Update(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.Category);
        
        product.IsActive = request.IsActive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map to response
        var response = _mapper.Map<ProductResponse>(product);

        return Result.Success(response);
    }
}
