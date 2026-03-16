using AutoMapper;
using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Domain.Abstractions;
using CatalogService.Domain.Product;
using CatalogService.Domain.Product.DTOs;

namespace CatalogService.Application.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, ProductsListResponse>
{
    private readonly IProductQueryService _productQueryService;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IProductQueryService productQueryService, IMapper mapper)
    {
        _productQueryService = productQueryService;
        _mapper = mapper;
    }

    public async Task<Result<ProductsListResponse>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        // Use the query service to get products with all filters applied
        var (products, totalCount) = await _productQueryService.GetProductsAsync(
            request.Parameters, 
            cancellationToken);

        // Map to DTOs
        var productDtos = _mapper.Map<List<ProductToReturnDto>>(products);

        var response = new ProductsListResponse
        {
            Products = productDtos,
            TotalCount = totalCount
        };

        return Result.Success(response);
    }
}
