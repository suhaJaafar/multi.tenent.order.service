using AutoMapper;
using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product;

namespace CatalogService.Application.GetProduct;

internal sealed class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProductResponse>> Handle(
        GetProductQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(ProductErrors.NotFound);
        }

        var response = _mapper.Map<ProductResponse>(product);

        return Result.Success(response);
    }
}

