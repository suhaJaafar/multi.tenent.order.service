namespace IdentityService.Domain.DTOs;
/// <summary>
/// DTO for orders when returned as part of a Product response.
/// Excludes ProductIds to avoid redundancy.
/// </summary>
public class OrderInProductDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreateAt { get; set; }
    public Guid UserId { get; set; }
}

