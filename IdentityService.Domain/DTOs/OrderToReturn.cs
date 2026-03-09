
namespace IdentityService.Domain.DTOs;
public class OrderToReturn
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreateAt { get; set; }
    public List<Guid> ProductIds { get; set; } = [];
    public Guid UserId { get; set; }
}