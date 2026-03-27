using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Api.Attributes;
using OrdersService.Application.CreateOrder;
using OrdersService.Application.GetOrder;
using OrdersService.Application.GetUserOrders;
using OrdersService.Domain.Order.DTOs;
using OrdersService.Infrastructure.Utilities;

namespace OrdersService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Create a new order for the authenticated user
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request,
        [ActiveUser] ActiveUserData activeUser,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (activeUser.Sub == Guid.Empty)
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            activeUser.Sub,
            request.Items,
            request.Notes);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));
        }

        return Ok(new ClientResponse<OrderResponse>(result.Value));
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{orderId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetOrder(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderQuery(orderId);

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(new { error = result.Error.Name, message = result.Error.Code });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Get all orders for the authenticated user
    /// </summary>
    [HttpGet("my-orders")]
    [Authorize]
    public async Task<IActionResult> GetMyOrders(
        [ModelBinder(typeof(ActiveUserModelBinder))] ActiveUserData activeUser,
        CancellationToken cancellationToken)
    {
        if (activeUser?.Sub == Guid.Empty)
        {
            return Unauthorized(new { error = "User not authenticated" });
        }

        var query = new GetUserOrdersQuery(activeUser.Sub);

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error.Name, message = result.Error.Code });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Get all orders for a specific user (Admin only - for future use)
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetUserOrders(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserOrdersQuery(userId);

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error.Name, message = result.Error.Code });
        }

        return Ok(result.Value);
    }
}
