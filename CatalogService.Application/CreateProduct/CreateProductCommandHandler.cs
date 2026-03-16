using AutoMapper;
using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Exceptions;
using CatalogService.Application.GetProduct;
using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product;
using CatalogService.Domain.Product.Entities;

namespace CatalogService.Application.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductResponse>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = Product.Create(
                request.Id,
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.OwnerUserId,
                request.Category);

            _productRepository.Add(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<ProductResponse>(product);

            return Result.Success(response); 
        }
        catch(ConcurrencyException)
        {
            return Result.Failure<ProductResponse>(ProductErrors.ConcurrencyConflict);
        }
        
    }
}

