using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using IdentityService.Domain.DTOs;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Identity.DTOs;
using IdentityService.Domain.Identity.Enums;

namespace IdentityService.Api.Attributes;
/// <summary>
/// Model binder for automatically binding the current authenticated user's data from JWT claims
/// </summary>
public class ActiveUserModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var httpContext = bindingContext.HttpContext;
        var user = httpContext.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var activeUserData = new ActiveUserData
        {
            Sub = GetUserId(user),
            Name = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
            Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            Role = GetUserRole(user),
            TenentName = GetTenentName(user)
        };

        bindingContext.Result = ModelBindingResult.Success(activeUserData);
        return Task.CompletedTask;
    }

    private static Guid GetUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst("id")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Guid.Empty;
        }

        return userId;
    }

    private static UserType GetUserRole(ClaimsPrincipal user)
    {
        var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value ?? user.FindFirst("feRole")?.Value;
        
        if (string.IsNullOrEmpty(roleClaim) || !Enum.TryParse<UserType>(roleClaim, out var userType))
        {
            return UserType.SuperAdmin; // Default fallback
        }

        return userType;
    }

    private static TenentName GetTenentName(ClaimsPrincipal user)
    {
        var tenentClaim = user.FindFirst("TenentName")?.Value ?? user.FindFirst("tenentName")?.Value;
        
        if (string.IsNullOrEmpty(tenentClaim) || !Enum.TryParse<TenentName>(tenentClaim, out var tenentName))
        {
            return TenentName.Default; // Default fallback
        }

        return tenentName;
    }
}

