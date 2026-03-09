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
}

