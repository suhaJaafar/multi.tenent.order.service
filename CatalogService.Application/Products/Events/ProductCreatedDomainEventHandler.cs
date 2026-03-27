using CatalogService.Domain.Product.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;

namespace CatalogService.Application.Products.Events;

internal sealed class ProductCreatedDomainEventHandler : INotificationHandler<ProductCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductCreatedDomainEventHandler> _logger;

    public ProductCreatedDomainEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<ProductCreatedDomainEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Publishing ProductCreatedIntegrationEvent for ProductId: {ProductId}",
            notification.ProductId);

        // Map domain event to integration event
        var integrationEvent = new ProductCreatedIntegrationEvent
        {
            ProductId = notification.ProductId,
            Name = notification.Name,
            Description = notification.Description,
            Price = notification.Price,
            Stock = notification.Stock,
            OwnerUserId = notification.OwnerUserId,
            Category = notification.Category.ToString(),
            IsActive = notification.IsActive,
            CreatedAt = DateTime.UtcNow,
            TenantId = Guid.Empty // TODO: Get from context
        };

        // Publish to RabbitMQ
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        _logger.LogInformation(
            "ProductCreatedIntegrationEvent published successfully for ProductId: {ProductId}",
            notification.ProductId);
    }
}

