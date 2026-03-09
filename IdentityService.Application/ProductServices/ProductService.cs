using AutoMapper;
using Microsoft.EntityFrameworkCore;
using IdentityService.Domain.DBContexts;
using IdentityService.Domain.DTOs;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Identity.Entities;
using IdentityService.Domain.FiltersParams;
using IdentityService.Domain.Forms;
using IdentityService.Infrastructure.Interfaces;
using IdentityService.Infrastructure.Utilities;

namespace IdentityService.Application.ProductServices;

public class ProductService : IProductService
{
    private readonly OSContext _context;
    private readonly IMapper _mapper;

    public ProductService(IMapper mapper, OSContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ServiceResponse<List<ProductsToReturn>>> GetAllProducts(ProductsParams productParams)
    {
        var query = _context.Products
            .OrderByDescending(p => p.CreateAt)
            .AsQueryable();

        if (!string.IsNullOrEmpty(productParams.SearchTerm))
        {
            query = query.Where(x =>
                x.Name.ToLower().Replace(" ", "").Contains(productParams.SearchTerm.Replace(" ", "").ToLower()) ||
                x.Description.ToLower().Replace(" ", "").Contains(productParams.SearchTerm.Replace(" ", "").ToLower()));
        }

        var count = await query.CountAsync();
        var result = await query
            .Skip(productParams.Skip)
            .Take(productParams.Take)
            .ToListAsync();

        var data = _mapper.Map<List<ProductsToReturn>>(result);
        return new ServiceResponse<List<ProductsToReturn>>(data, count);
    }

    public async Task<ServiceResponse<Product>> AddProduct(CreateProductForm form)
    {
        if (string.IsNullOrWhiteSpace(form.Name))
            return new ServiceResponse<Product>(true, "Product name is required");

        var existingProduct = await _context.Products
            .Where(x => x.Name.ToLower() == form.Name.ToLower())
            .FirstOrDefaultAsync();

        if (existingProduct != null)
            return new ServiceResponse<Product>(true, "Product with this name already exists");

        var product = _mapper.Map<Product>(form);
        product.CreateAt = DateTime.UtcNow;

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return new ServiceResponse<Product>(product);
    }

    public async Task<ServiceResponse<List<ProductsToReturn>>> AddProductAsFavorite(Guid userId, Guid productId)
    {
        var user = await _context.Users
            .Include(u => u.FavoriteProducts)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new ServiceResponse<List<ProductsToReturn>>(true, "User not found");

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
            return new ServiceResponse<List<ProductsToReturn>>(true, "Product not found");

        if (user.FavoriteProducts.Any(p => p.Id == productId))
            return new ServiceResponse<List<ProductsToReturn>>(true, "Product is already in favorites");

        user.FavoriteProducts.Add(product);
        await _context.SaveChangesAsync();

        var favoriteProducts = _mapper.Map<List<ProductsToReturn>>(user.FavoriteProducts);
        return new ServiceResponse<List<ProductsToReturn>>(favoriteProducts);
    }

    public async Task<ServiceResponse<List<ProductsToReturn>>> RemoveProductFromFavorites(Guid userId, Guid productId)
    {
        var user = await _context.Users
            .Include(u => u.FavoriteProducts)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new ServiceResponse<List<ProductsToReturn>>(true, "User not found");

        var product = user.FavoriteProducts.FirstOrDefault(p => p.Id == productId);
        if (product == null)
            return new ServiceResponse<List<ProductsToReturn>>(true, "Product not found in favorites");

        user.FavoriteProducts.Remove(product);
        await _context.SaveChangesAsync();

        var favoriteProducts = _mapper.Map<List<ProductsToReturn>>(user.FavoriteProducts);
        return new ServiceResponse<List<ProductsToReturn>>(favoriteProducts);
    }

    public async Task<ServiceResponse<List<ProductsToReturn>>> GetFavoriteProducts(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.FavoriteProducts)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new ServiceResponse<List<ProductsToReturn>>(true, "User not found");

        var favoriteProducts = _mapper.Map<List<ProductsToReturn>>(user.FavoriteProducts);
        return new ServiceResponse<List<ProductsToReturn>>(favoriteProducts, favoriteProducts.Count);
    }
}

