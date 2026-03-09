using System.ComponentModel;

namespace CatalogService.Domain.Product.Enums;

public enum ProductCategory
{
    [Description("General")]
    General = 0,

    [Description("Electronics")]
    Electronics = 1,

    [Description("Clothing")]
    Clothing = 2,

    [Description("Food")]
    Food = 3,

    [Description("Furniture")]
    Furniture = 4,

    [Description("Other")]
    Other = 5
}

