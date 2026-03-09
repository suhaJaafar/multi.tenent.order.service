using IdentityService.Domain.Enums;

namespace IdentityService.Domain.DTOs;
public class OrderDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreateAt { get; set; }
    public List<ProductDto> Products { get; set; } = [];
}

