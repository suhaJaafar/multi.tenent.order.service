using System.ComponentModel.DataAnnotations;
using MultiTenantOrderService.Domain.Enums;

namespace MultiTenantOrderService.Domain.Forms;
public class CreateOrderForm
{
    [Required]
    public Guid ProductId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public OrderStatus OrderStatus { get; set; }
}