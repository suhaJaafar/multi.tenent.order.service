using System.ComponentModel.DataAnnotations;

namespace CatalogService.Domain.Product.Forms;

public class UpdateProductForm : CreateProductForm
{
    [Required]
    public Guid Id { get; set; }
}

