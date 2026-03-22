using IdentityService.Domain.DTOs;
using IdentityService.Domain.Entities;
using IdentityService.Domain.FiltersParams;
using IdentityService.Domain.Forms;

namespace IdentityService.Application.Abstractions;

public interface IProductService
{
    Task<ServiceResponse<List<ProductsToReturn>>> GetAllProducts(ProductsParams productParams);
    Task<ServiceResponse<Product>> AddProduct(CreateProductForm form);
    Task<ServiceResponse<List<ProductsToReturn>>> AddProductAsFavorite(Guid userId, Guid productId);
    Task<ServiceResponse<List<ProductsToReturn>>> RemoveProductFromFavorites(Guid userId, Guid productId);
    Task<ServiceResponse<List<ProductsToReturn>>> GetFavoriteProducts(Guid userId);
}

