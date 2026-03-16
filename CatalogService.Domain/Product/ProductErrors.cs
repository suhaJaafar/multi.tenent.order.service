using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Product;

public static class ProductErrors
{
    public static Error NotFound = new(
        "Product.NotFound",
        "The product with the specified identifier was not found");

    public static Error Unauthorized = new(
        "Product.Unauthorized",
        "You are not authorized to modify this product");

    public static Error InvalidPrice = new(
        "Product.InvalidPrice",
        "The product price must be greater than zero");

    public static Error InvalidStock = new(
        "Product.InvalidStock",
        "The product stock cannot be negative");

    public static Error ConcurrencyConflict = new(
        "Product.ConcurrencyConflict",
        "The product was modified by another user. Please refresh and try again");
}