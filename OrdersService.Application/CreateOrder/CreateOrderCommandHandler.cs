using AutoMapper;
using OrdersService.Application.Abstractions.Messaging;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Order;
using OrdersService.Domain.Order.Entities;
using OrdersService.Domain.Product;

namespace OrdersService.Application.CreateOrder;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductReferenceRepository _productReferenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductReferenceRepository productReferenceRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productReferenceRepository = productReferenceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // Validate user ID
        if (request.UserId == Guid.Empty)
        {
            return Result.Failure<OrderResponse>(OrderErrors.InvalidUser);
        }

        // Validate that order has items
        if (request.Items == null || !request.Items.Any())
        {
            return Result.Failure<OrderResponse>(OrderErrors.NoItemsProvided);
        }

        // Create order (without items first)
        var order = Order.Create(
            request.Id,
            request.UserId,
            request.Notes);

        // Get all product IDs
        var productIds = request.Items.Select(i => i.ProductId).ToList();

        // Get products from local reference (synced via events from CatalogService)
        var productReferences = await _productReferenceRepository.GetByIdsAsync(
            productIds,
            cancellationToken);

        // Validate all products exist
        foreach (var item in request.Items)
        {
            var productRef = productReferences.FirstOrDefault(p => p.Id == item.ProductId);

            if (productRef == null)
            {
                return Result.Failure<OrderResponse>(
                    OrderErrors.ProductNotFound(item.ProductId));
            }

            if (!productRef.IsOrderable())
            {
                return Result.Failure<OrderResponse>(
                    OrderErrors.ProductNotAvailable(item.ProductId));
            }

            // Validate quantity
            if (item.Quantity <= 0)
            {
                return Result.Failure<OrderResponse>(
                    OrderErrors.InvalidQuantity(item.ProductId));
            }

            // Create order item with product snapshot
            // This preserves product details at order time (price, name, SKU)
            var orderItem = OrderItem.Create(
                Guid.NewGuid(),
                order.Id,
                productRef.Id,
                productRef.SKU,
                productRef.Name,
                productRef.CurrentPrice,
                item.Quantity);

            order.AddOrderItem(orderItem);
        }

        // Confirm order and raise domain event
        order.Confirm();

        _orderRepository.Add(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map the created order to OrderResponse
        var response = _mapper.Map<OrderResponse>(order);

        return Result.Success(response);
    }
}



