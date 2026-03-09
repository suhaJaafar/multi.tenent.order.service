using MediatR;
using Microsoft.Extensions.Logging;
using OrdersService.Domain.Order.Events;

namespace OrdersService.Application.Orders.Events;

internal sealed class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly ILogger<OrderCreatedDomainEventHandler> _logger;

    public OrderCreatedDomainEventHandler(ILogger<OrderCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Order Created: OrderId={OrderId}, UserId={UserId}, Amount={Amount}",
            notification.OrderId,
            notification.UserId,
            notification.TotalAmount);

        // Here you can add additional logic:
        // - Send email notification
        // - Publish integration event to message bus
        // - Update analytics/reporting
        // - Trigger other business processes

        return Task.CompletedTask;
    }
}

