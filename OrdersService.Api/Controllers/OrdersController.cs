using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Api.Attributes;
using OrdersService.Api.DTOs;
using OrdersService.Application.CreateOrder;
using OrdersService.Application.GetOrder;
using OrdersService.Application.GetUserOrders;

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
        [ModelBinder(typeof(ActiveUserModelBinder))] ActiveUserData activeUser,
        CancellationToken cancellationToken)
    {
        if (activeUser?.UserId == Guid.Empty)
        {
            return Unauthorized(new { error = "User not authenticated" });
        }

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            activeUser.UserId, // Use authenticated user's ID
            request.TotalAmount,
            request.Notes);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error.Name, message = result.Error.Code });
        }

        return CreatedAtAction(
            nameof(GetOrder),
            new { orderId = result.Value.Id },
            result.Value);
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
        if (activeUser?.UserId == Guid.Empty)
        {
            return Unauthorized(new { error = "User not authenticated" });
        }

        var query = new GetUserOrdersQuery(activeUser.UserId);

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

public record CreateOrderRequest(
    decimal TotalAmount,
    string? Notes);

