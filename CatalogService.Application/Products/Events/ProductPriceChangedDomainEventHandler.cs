using CatalogService.Domain.Product.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;

namespace CatalogService.Application.Products.Events;

internal sealed class ProductPriceChangedDomainEventHandler : INotificationHandler<ProductPriceChangedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductPriceChangedDomainEventHandler> _logger;

    public ProductPriceChangedDomainEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<ProductPriceChangedDomainEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(ProductPriceChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Publishing ProductPriceChangedIntegrationEvent for ProductId: {ProductId}, OldPrice: {OldPrice}, NewPrice: {NewPrice}",
            notification.ProductId,
            notification.OldPrice,
            notification.NewPrice);

        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.ProductId,
            OldPrice = notification.OldPrice,
            NewPrice = notification.NewPrice,
            ChangedAt = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        _logger.LogInformation(
            "ProductPriceChangedIntegrationEvent published successfully for ProductId: {ProductId}",
            notification.ProductId);
    }
}

