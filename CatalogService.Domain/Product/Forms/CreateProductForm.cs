using System.ComponentModel.DataAnnotations;
using CatalogService.Domain.Product.Enums;

namespace CatalogService.Domain.Product.Forms;

public class CreateProductForm
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
    public int Stock { get; set; }

    [Required]
    public ProductCategory Category { get; set; }
}

