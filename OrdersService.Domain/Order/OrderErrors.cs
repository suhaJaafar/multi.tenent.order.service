using OrdersService.Domain.Abstractions;

namespace OrdersService.Domain.Order;

public static class OrderErrors
{
    public static Error NotFound = new(
        "Order.NotFound",
        "The order with the specified identifier was not found");

    public static Error InvalidAmount = new(
        "Order.InvalidAmount",
        "The order amount must be greater than zero");

    public static Error InvalidUser = new(
        "Order.InvalidUser",
        "The user identifier is invalid");

    public static Error AlreadyCancelled = new(
        "Order.AlreadyCancelled",
        "The order has already been cancelled");

    public static Error CannotCancel = new(
        "Order.CannotCancel",
        "The order cannot be cancelled in its current status");

    public static Error NoItemsProvided = new(
        "Order.NoItemsProvided",
        "Order must contain at least one item");

    public static Error ProductNotFound(Guid productId) => new(
        "Order.ProductNotFound",
        $"Product with ID {productId} not found in local reference");

    public static Error ProductNotAvailable(Guid productId) => new(
        "Order.ProductNotAvailable",
        $"Product with ID {productId} is not available for ordering");

    public static Error InvalidQuantity(Guid productId) => new(
        "Order.InvalidQuantity",
        $"Invalid quantity for product {productId}");
}
