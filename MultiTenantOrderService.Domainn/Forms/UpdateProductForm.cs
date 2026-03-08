using System.ComponentModel.DataAnnotations;

namespace MultiTenantOrderService.Domain.Forms;
public class UpdateProductForm : CreateProductForm
{
    [Required]
    public Guid Id { get; set; }
}