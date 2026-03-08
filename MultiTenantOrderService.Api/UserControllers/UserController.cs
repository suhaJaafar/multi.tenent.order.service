using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using MultiTenantOrderService.Domain.Identity.Entities;
using MultiTenantOrderService.Infrastructure.Interfaces;
using MultiTenantOrderService.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MultiTenantOrderService.Application.CreateUser;
using MultiTenantOrderService.Application.GetUser;
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

    private readonly IUserService _userService;

    
    private readonly ISender _sender;
    
    public UserController(IUserService userService, IConfiguration configuration, ISender sender)
    {
        _sender = sender;
        _userService = userService;
        _configuration = configuration;
    }
    
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginForm form)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var serviceResponse = await _userService.Login(form.Email, form.Password);
        if (serviceResponse.Error) return Unauthorized();
    
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, serviceResponse.Value.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, serviceResponse.Value.Email.Value),
            new(ClaimTypes.Role, serviceResponse.Value.UserType.ToString()),
            new("feRole", serviceResponse.Value.UserType.ToString()),
            new(ClaimTypes.Name, serviceResponse.Value.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("id", serviceResponse.Value.Id.ToString()),
            new("TenentName", serviceResponse.Value.TenentName.ToString())
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
        });
    }
    
    // get current logged-in user info
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUserInfo([ActiveUser] ActiveUserData activeUser)
    {
        if (activeUser.Sub == Guid.Empty)
            return Unauthorized(new ClientResponse<string>(true, "User not authenticated"));
        
        var serviceResponse = await _userService.GetUserById(activeUser.Sub);
        if (serviceResponse.Error)
            return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
        return Ok(new ClientResponse<User>(serviceResponse.Value));
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
    
    
    //
    // [Authorize(Roles = "SuperAdmin")]
    // [HttpGet]
    // public async Task<ActionResult<IList<UsersToReturnDto>>> GetUsers([FromQuery] UsersParams userParams)
    // {
    //     var serviceResponse = await _userService.GetUsers(userParams);
    //     if (serviceResponse.Error)
    //         return BadRequest(new ClientResponse<List<UsersToReturnDto>>(true, serviceResponse.ResponseMessage));
    //     return Ok(new ClientResponse<List<UsersToReturnDto>>(serviceResponse.Value, serviceResponse.Count));
    // }
    //
    // // [Authorize(Roles = "SuperAdmin")]
    // [AllowAnonymous]
    // [HttpPost]
    // public async Task<IActionResult> AddUser(CreateUserForm form)
    // {
    //     if (!ModelState.IsValid) return BadRequest(ModelState);
    //
    //     var serviceResponse = await _userService.AddUser(form);
    //     if (serviceResponse.Error) return BadRequest(new ClientResponse<string>(true, serviceResponse.ResponseMessage));
    //
    //     return Ok(new ClientResponse<User>(serviceResponse.Value));
    // }
    //
    //
    // [Authorize(Roles = "SuperAdmin")]
    // [HttpPut]
    // public async Task<IActionResult> UpdateUser(UpdateUserForm form)
    // {
    //     var serviceResponse = await _userService.UpdateUser(form);
    //     if (serviceResponse.Error) return BadRequest(new ClientResponse<bool>(true, serviceResponse.ResponseMessage));
    //     return Ok(new ClientResponse<User>(serviceResponse.Value));
    // }

    
   
    
    
}