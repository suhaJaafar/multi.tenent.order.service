using MassTransit;
using Microsoft.Extensions.Logging;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Product;
using Shared.Contracts.Events;

namespace OrdersService.Application.Products.Consumers;

public class ProductArchivedIntegrationEventConsumer : IConsumer<ProductArchivedIntegrationEvent>
{
    private readonly IProductReferenceRepository _productReferenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductArchivedIntegrationEventConsumer> _logger;

    public ProductArchivedIntegrationEventConsumer(
        IProductReferenceRepository productReferenceRepository,
        IUnitOfWork unitOfWork,
        ILogger<ProductArchivedIntegrationEventConsumer> logger)
    {
        _productReferenceRepository = productReferenceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductArchivedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received ProductArchivedIntegrationEvent for ProductId: {ProductId}",
            message.ProductId);

        try
        {
            var productReference = await _productReferenceRepository.GetByIdAsync(
                message.ProductId,
                context.CancellationToken);

            if (productReference == null)
            {
                _logger.LogWarning(
                    "Product {ProductId} not found in local reference. Cannot archive.",
                    message.ProductId);
                return;
            }

            productReference.Archive();
            _productReferenceRepository.Update(productReference);

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation(
                "Successfully archived product reference for ProductId: {ProductId}",
                message.ProductId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error processing ProductArchivedIntegrationEvent for ProductId: {ProductId}",
                message.ProductId);
            throw;
        }
    }
}

