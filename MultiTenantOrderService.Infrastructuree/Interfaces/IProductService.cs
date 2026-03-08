using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.Entities;
using MultiTenantOrderService.Domain.FiltersParams;
using MultiTenantOrderService.Domain.Forms;
using MultiTenantOrderService.Infrastructure.Utilities;

namespace MultiTenantOrderService.Infrastructur.Interfaces;

public interface IProductService
{
    Task<ServiceResponse<List<ProductsToReturn>>> GetAllProducts(ProductsParams productParams);
    Task<ServiceResponse<Product>> AddProduct(CreateProductForm form);
    Task<ServiceResponse<List<ProductsToReturn>>> AddProductAsFavorite(Guid userId, Guid productId);
    Task<ServiceResponse<List<ProductsToReturn>>> RemoveProductFromFavorites(Guid userId, Guid productId);
    Task<ServiceResponse<List<ProductsToReturn>>> GetFavoriteProducts(Guid userId);
}