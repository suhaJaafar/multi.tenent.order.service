using MediatR;
using CatalogService.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CatalogService.Api.Attributes;
using CatalogService.Application.CreateProduct;
using CatalogService.Application.DeleteProduct;
using CatalogService.Application.GetProduct;
using CatalogService.Application.GetProducts;
using CatalogService.Application.UpdateProduct;
using CatalogService.Domain.Product.DTOs;
using CatalogService.Domain.Product.FiltersParams;

namespace CatalogService.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct(
        [ActiveUser] ActiveUserData activeUser,
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (activeUser.Sub == Guid.Empty)
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));

        var command = new CreateProductCommand(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            activeUser.Sub, // OwnerUserId from JWT (Identity service user)
            request.Category);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));
        }

        return Ok(new ClientResponse<ProductResponse>(result.Value));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(new ClientResponse<string>(true, result.Error.Name));

        return Ok(new ClientResponse<ProductResponse>(result.Value));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts([FromQuery] ProductsParams productParams, CancellationToken cancellationToken)
    {
        var query = new GetProductsQuery(productParams);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));

        return Ok(new ClientResponse<List<ProductToReturnDto>>(result.Value.Products, result.Value.TotalCount));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetMyProducts(
        [ActiveUser] ActiveUserData activeUser,
        [FromQuery] ProductsParams productParams,
        CancellationToken cancellationToken)
    {
        if (activeUser.Sub == Guid.Empty)
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));

        // Force filter to current user's products only
        productParams.OwnerUserId = activeUser.Sub;

        var query = new GetProductsQuery(productParams);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));

        return Ok(new ClientResponse<List<ProductToReturnDto>>(result.Value.Products, result.Value.TotalCount));
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(
        [ActiveUser] ActiveUserData activeUser,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (activeUser.Sub == Guid.Empty)
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));

        var command = new UpdateProductCommand(
            request.Id,
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            activeUser.Sub, // OwnerUserId from JWT for ownership verification
            request.Category,
            request.IsActive);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));

        return Ok(new ClientResponse<ProductResponse>(result.Value));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(
        [ActiveUser] ActiveUserData activeUser,
        Guid id,
        CancellationToken cancellationToken)
    {
        if (activeUser.Sub == Guid.Empty)
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));

        var command = new DeleteProductCommand(id, activeUser.Sub);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));

        return Ok(new ClientResponse<string>("Product deleted successfully"));
    }
}

