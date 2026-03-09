namespace OrdersService.Api.DTOs;

/// <summary>
/// Represents the currently authenticated user's data extracted from JWT claims
/// </summary>
public class ActiveUserData
{
    /// <summary>
    /// User ID (from "sub" or "id" claim)
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// User's name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// User's email
    /// </summary>
    public string Email { get; set; } = string.Empty;
}

