using CatalogService.Domain.Product.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;

namespace CatalogService.Application.Products.Events;

internal sealed class ProductUpdatedDomainEventHandler : INotificationHandler<ProductUpdatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductUpdatedDomainEventHandler> _logger;

    public ProductUpdatedDomainEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<ProductUpdatedDomainEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(ProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Publishing ProductUpdatedIntegrationEvent for ProductId: {ProductId}",
            notification.ProductId);

        var integrationEvent = new ProductUpdatedIntegrationEvent
        {
            ProductId = notification.ProductId,
            Name = notification.Name,
            Description = notification.Description,
            Price = notification.Price,
            Stock = notification.Stock,
            Category = notification.Category.ToString(),
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        _logger.LogInformation(
            "ProductUpdatedIntegrationEvent published successfully for ProductId: {ProductId}",
            notification.ProductId);
    }
}
