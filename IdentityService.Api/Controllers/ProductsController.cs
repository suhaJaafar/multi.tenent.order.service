using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IdentityService.Domain.DTOs;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Identity.Entities;
using IdentityService.Domain.FiltersParams;
using IdentityService.Domain.Forms;
using IdentityService.Infrastructure.Interfaces;
using IdentityService.Infrastructure.Utilities;

namespace IdentityService.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    public async Task<ActionResult<IList<ProductsToReturn>>> GetAllProducts([FromQuery] ProductsParams productsParams)
    {
        var products = await _productService.GetAllProducts(productsParams);
        return Ok(new ClientResponse<IList<ProductsToReturn>>(products.Value, products.Count));
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost]
    public async Task<IActionResult> AddProduct(CreateProductForm form)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
    
        var serviceResponse = await _productService.AddProduct(form);
        if (serviceResponse.Error) return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
    
        return Ok(new ClientResponse<Product>(serviceResponse.Value));
    }
    
    // Get favorite products for the logged-in user
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IList<ProductsToReturn>>> GetFavoriteProducts()
    {
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var loggedinUserId))
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));
        
        var serviceResponse = await _productService.GetFavoriteProducts(loggedinUserId);
        if (serviceResponse.Error)
            return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
        
        return Ok(new ClientResponse<List<ProductsToReturn>>(serviceResponse.Value));
    }
    
    // Add a product to favorites for the logged-in user by using userId from token
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddProductAsFavorite([FromBody] Guid productId)
    {
        var userIdString = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));
        
        var serviceResponse = await _productService.AddProductAsFavorite(userId, productId);
        if (serviceResponse.Error)
            return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
        
        return Ok(new ClientResponse<List<ProductsToReturn>>(serviceResponse.Value));
    }
    
    // Remove a product from favorites for the logged-in user
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveProductFromFavorites([FromQuery] Guid productId)
    {
        var userIdString = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));
        
        var serviceResponse = await _productService.RemoveProductFromFavorites(userId, productId);
        if (serviceResponse.Error)
            return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
        
        return Ok(new ClientResponse<List<ProductsToReturn>>(serviceResponse.Value));
    }
}