using MassTransit;
using Microsoft.Extensions.Logging;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Product;
using OrdersService.Domain.Product.Entities;
using Shared.Contracts.Events;

namespace OrdersService.Application.Products.Consumers;

/// <summary>
/// Consumes ProductCreatedIntegrationEvent from Catalog Service
/// and maintains local product reference data
/// </summary>
public class ProductCreatedIntegrationEventConsumer : IConsumer<ProductCreatedIntegrationEvent>
{
    private readonly IProductReferenceRepository _productReferenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductCreatedIntegrationEventConsumer> _logger;

    public ProductCreatedIntegrationEventConsumer(
        IProductReferenceRepository productReferenceRepository,
        IUnitOfWork unitOfWork,
        ILogger<ProductCreatedIntegrationEventConsumer> logger)
    {
        _productReferenceRepository = productReferenceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received ProductCreatedIntegrationEvent for ProductId: {ProductId}, Name: {Name}",
            message.ProductId,
            message.Name);

        try
        {
            // Check if product already exists (idempotency)
            var existingProduct = await _productReferenceRepository.GetByIdAsync(
                message.ProductId,
                context.CancellationToken);

            if (existingProduct != null)
            {
                _logger.LogWarning(
                    "Product {ProductId} already exists in local reference. Updating instead.",
                    message.ProductId);

                existingProduct.Update(message.Name, message.Price, message.Category, message.IsActive);
                _productReferenceRepository.Update(existingProduct);
            }
            else
            {
                // Create new product reference
                var productReference = ProductReference.Create(
                    message.ProductId,
                    message.ProductId.ToString()[..8], // Use first 8 chars of GUID as SKU for now
                    message.Name,
                    message.Price,
                    message.Category,
                    message.IsActive,
                    message.OwnerUserId);

                _productReferenceRepository.Add(productReference);

                _logger.LogInformation(
                    "Created product reference for ProductId: {ProductId}",
                    message.ProductId);
            }

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation(
                "Successfully processed ProductCreatedIntegrationEvent for ProductId: {ProductId}",
                message.ProductId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error processing ProductCreatedIntegrationEvent for ProductId: {ProductId}",
                message.ProductId);
            throw; // Let MassTransit handle retry
        }
    }
}

