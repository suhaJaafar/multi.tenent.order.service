using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.Forms;
public class UpdateProductForm : CreateProductForm
{
    [Required]
    public Guid Id { get; set; }
}