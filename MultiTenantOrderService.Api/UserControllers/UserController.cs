using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using MultiTenantOrderService.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MultiTenantOrderService.Application.CreateUser;
using MultiTenantOrderService.Application.GetUser;
using MultiTenantOrderService.Application.GetUsers;
using MultiTenantOrderService.Application.LoginUser;
using MultiTenantOrderService.Application.UpdateUser;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.DTOs;
using MultiTenantOrderService.Domain.Identity.Enums;
using MultiTenantOrderService.Domain.Identity.FiltersParams;
using MultiTenantOrderService.Domain.Identity.Forms;

namespace MultiTenantOrderService.Api.UserControllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ISender _sender;
    
    public UserController(IConfiguration configuration, ISender sender)
    {
        _sender = sender;
        _configuration = configuration;
    }
    
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var command = new LoginUserCommand(form.Email, form.Password);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(new { error = result.Error.Name });
        }

        var user = result.Value;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.UserType),
            new("feRole", user.UserType),
            new(ClaimTypes.Name, user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("id", user.UserId.ToString()),
            new("TenentName", user.TenentName)
        };
        
        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.UtcNow.AddDays(3),
            notBefore: DateTime.UtcNow,
            audience: _configuration["JWT:Audience"],
            issuer: _configuration["JWT:Issuer"],
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!)),
                SecurityAlgorithms.HmacSha256)
        );
    
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            user
        });
    }
    
    // get current logged-in user info
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUserInfo([ActiveUser] ActiveUserData activeUser, CancellationToken cancellationToken)
    {
        if (activeUser.Sub == Guid.Empty)
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));

        var query = new GetUserQuery(activeUser.Sub);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));

        return Ok(new ClientResponse<UserResponse>(result.Value));
    }
    
    
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            Guid.NewGuid(),
            request.Name,
            request.Email,
            request.Password,
            request.UserType,
            request.TenentName);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        // Return the full user information instead of just the ID
        return CreatedAtAction(
            nameof(GetUser), 
            new { id = result.Value.Id }, 
            result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }
    
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UsersParams userParams, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(userParams);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));

        return Ok(new ClientResponse<List<UsersToReturnDto>>(result.Value.Users, result.Value.TotalCount));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var command = new UpdateUserCommand(
            request.Id,
            request.Name,
            request.Email,
            request.Password,
            request.PhoneNumber,
            request.UserType,
            request.TenentName);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new ClientResponse<string>(true, result.Error.Name));

        return Ok(new ClientResponse<UserResponse>(result.Value));
    }
}
