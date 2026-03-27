using MassTransit;
using Microsoft.Extensions.Logging;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Product;
using Shared.Contracts.Events;

namespace OrdersService.Application.Products.Consumers;

public class ProductPriceChangedIntegrationEventConsumer : IConsumer<ProductPriceChangedIntegrationEvent>
{
    private readonly IProductReferenceRepository _productReferenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductPriceChangedIntegrationEventConsumer> _logger;

    public ProductPriceChangedIntegrationEventConsumer(
        IProductReferenceRepository productReferenceRepository,
        IUnitOfWork unitOfWork,
        ILogger<ProductPriceChangedIntegrationEventConsumer> logger)
    {
        _productReferenceRepository = productReferenceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received ProductPriceChangedIntegrationEvent for ProductId: {ProductId}, OldPrice: {OldPrice}, NewPrice: {NewPrice}",
            message.ProductId,
            message.OldPrice,
            message.NewPrice);

        try
        {
            var productReference = await _productReferenceRepository.GetByIdAsync(
                message.ProductId,
                context.CancellationToken);

            if (productReference == null)
            {
                _logger.LogWarning(
                    "Product {ProductId} not found in local reference. Cannot update price.",
                    message.ProductId);
                return;
            }

            productReference.UpdatePrice(message.NewPrice);
            _productReferenceRepository.Update(productReference);

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation(
                "Successfully updated price for ProductId: {ProductId} to {NewPrice}",
                message.ProductId,
                message.NewPrice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error processing ProductPriceChangedIntegrationEvent for ProductId: {ProductId}",
                message.ProductId);
            throw;
        }
    }
}

