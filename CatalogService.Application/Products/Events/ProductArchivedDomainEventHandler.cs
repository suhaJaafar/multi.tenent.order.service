using CatalogService.Domain.Product.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;

namespace CatalogService.Application.Products.Events;

internal sealed class ProductArchivedDomainEventHandler : INotificationHandler<ProductArchivedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductArchivedDomainEventHandler> _logger;

    public ProductArchivedDomainEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<ProductArchivedDomainEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(ProductArchivedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Publishing ProductArchivedIntegrationEvent for ProductId: {ProductId}",
            notification.ProductId);

        var integrationEvent = new ProductArchivedIntegrationEvent
        {
            ProductId = notification.ProductId,
            ArchivedAt = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        _logger.LogInformation(
            "ProductArchivedIntegrationEvent published successfully for ProductId: {ProductId}",
            notification.ProductId);
    }
}

