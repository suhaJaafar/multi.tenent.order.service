namespace CatalogService.Domain.Product.DTOs;

/// <summary>
/// Represents the currently authenticated user's data extracted from JWT claims.
/// The user originates from the Identity microservice.
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
    /// User's role/type (string from Identity service)
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Tenant name associated with the user
    /// </summary>
    public string TenentName { get; set; } = string.Empty;
}

