using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Domain.Abstractions;
using CatalogService.Domain.DBContexts;
using CatalogService.Domain.Product.DTOs;

namespace CatalogService.Application.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, ProductsListResponse>
{
    private readonly CatalogContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(CatalogContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<ProductsListResponse>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var specParams = request.Parameters;
        var query = _context.Products.OrderByDescending(v => v.CreateAt).AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(specParams.SearchTerm))
        {
            var searchTerm = specParams.SearchTerm.ToLower().Replace(" ", "");
            query = query.Where(x =>
                x.Name.ToLower().Replace(" ", "").Contains(searchTerm) ||
                x.Description.ToLower().Replace(" ", "").Contains(searchTerm));
        }

        // Apply filters
        if (specParams.Id.HasValue)
            query = query.Where(x => x.Id == specParams.Id);

        if (specParams.Category.HasValue)
            query = query.Where(x => x.Category == specParams.Category);

        if (specParams.OwnerUserId.HasValue)
            query = query.Where(x => x.OwnerUserId == specParams.OwnerUserId);

        if (specParams.IsActive.HasValue)
            query = query.Where(x => x.IsActive == specParams.IsActive);

        // Get total count
        var count = await query.CountAsync(cancellationToken);

        // Apply pagination
        var products = await query
            .Skip(specParams.Skip)
            .Take(specParams.Take)
            .ToListAsync(cancellationToken);

        // Map to DTOs
        var productDtos = _mapper.Map<List<ProductToReturnDto>>(products);

        var response = new ProductsListResponse
        {
            Products = productDtos,
            TotalCount = count
        };

        return Result.Success(response);
    }
}

