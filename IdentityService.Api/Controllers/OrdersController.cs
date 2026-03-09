using IdentityService.Domain.DTOs;
using IdentityService.Domain.FiltersParams;
using IdentityService.Domain.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Domain.Identity.DTOs;
using IdentityService.Infrastructure.Utilities;

namespace IdentityService.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
   
    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    public async Task<ActionResult<IList<OrderToReturn>>> GetOrdersWithProducts([FromQuery] OrdersParams ordersParams, [ActiveUser] ActiveUserData activeUser)
    {
        var serviceResponse = await _orderService.GetOrdersWithProducts(ordersParams, activeUser);
        if (serviceResponse.Error) return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
        return Ok(new ClientResponse<IList<OrderToReturn>>(serviceResponse.Value));
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost]
    public async Task<IActionResult> AddOrder(CreateOrderForm createOrderForm, [ActiveUser] ActiveUserData activeUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var serviceResponse = await _orderService.AddOrder(createOrderForm, activeUser);
        if (serviceResponse.Error) return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
        return Ok(new ClientResponse<OrderDto>(serviceResponse.Value));
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(Guid id, UpdateOrderForm updateOrderForm, [ActiveUser] ActiveUserData activeUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var serviceResponse = await _orderService.UpdateOrder(id, updateOrderForm, activeUser);
        if (serviceResponse.Error) return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
        return Ok(new ClientResponse<OrderDto>(serviceResponse.Value));
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        var serviceResponse = await _orderService.DeleteOrder(id);
        if (serviceResponse.Error) return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
        return Ok(new ClientResponse<bool>(serviceResponse.Value));
    }
    
}