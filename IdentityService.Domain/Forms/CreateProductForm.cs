using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.Forms;
public class CreateProductForm
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    public int? Stock { get; set; }
    
    public string? ImageUrl { get; set; }
}