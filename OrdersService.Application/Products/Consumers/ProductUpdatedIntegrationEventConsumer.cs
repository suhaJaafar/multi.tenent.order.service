using MassTransit;
using Microsoft.Extensions.Logging;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Product;
using Shared.Contracts.Events;

namespace OrdersService.Application.Products.Consumers;

public class ProductUpdatedIntegrationEventConsumer : IConsumer<ProductUpdatedIntegrationEvent>
{
    private readonly IProductReferenceRepository _productReferenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductUpdatedIntegrationEventConsumer> _logger;

    public ProductUpdatedIntegrationEventConsumer(
        IProductReferenceRepository productReferenceRepository,
        IUnitOfWork unitOfWork,
        ILogger<ProductUpdatedIntegrationEventConsumer> logger)
    {
        _productReferenceRepository = productReferenceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductUpdatedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received ProductUpdatedIntegrationEvent for ProductId: {ProductId}",
            message.ProductId);

        try
        {
            var productReference = await _productReferenceRepository.GetByIdAsync(
                message.ProductId,
                context.CancellationToken);

            if (productReference == null)
            {
                _logger.LogWarning(
                    "Product {ProductId} not found in local reference. Cannot update.",
                    message.ProductId);
                return;
            }

            productReference.Update(message.Name, message.Price, message.Category, message.IsActive);
            _productReferenceRepository.Update(productReference);

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation(
                "Successfully updated product reference for ProductId: {ProductId}",
                message.ProductId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error processing ProductUpdatedIntegrationEvent for ProductId: {ProductId}",
                message.ProductId);
            throw;
        }
    }
}

