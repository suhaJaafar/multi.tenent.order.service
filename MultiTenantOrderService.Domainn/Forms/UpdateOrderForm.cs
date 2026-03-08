using System.ComponentModel.DataAnnotations;

namespace MultiTenantOrderService.Domain.Forms;
public class UpdateOrderForm
{
    public Guid? ProductId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
}