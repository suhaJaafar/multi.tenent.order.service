using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Domain.Identity.DTOs;
/// <summary>
/// Represents the currently authenticated user's data extracted from JWT claims
/// </summary>
public class ActiveUserData
{
    /// <summary>
    /// User ID (from "id" or "sub" claim)
    /// </summary>
    public Guid Sub { get; set; }
    
    /// <summary>
    /// User's name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// User's email
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// User's role/type
    /// </summary>
    public UserType Role { get; set; }
    
    /// <summary>
    /// Tenant name associated with the user
    /// </summary>
    public TenentName TenentName { get; set; }
}

