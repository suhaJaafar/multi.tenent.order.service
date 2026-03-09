using AutoMapper;
using OrdersService.Application.Abstractions.Messaging;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Order;
using OrdersService.Domain.Order.Entities;

namespace OrdersService.Application.CreateOrder;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // Validate amount
        if (request.TotalAmount <= 0)
        {
            return Result.Failure<OrderResponse>(OrderErrors.InvalidAmount);
        }

        // Validate user ID
        if (request.UserId == Guid.Empty)
        {
            return Result.Failure<OrderResponse>(OrderErrors.InvalidUser);
        }

        // Create order with domain event
        var order = Order.Create(
            request.Id,
            request.UserId,
            request.TotalAmount,
            request.Notes);

        _orderRepository.Add(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map the created order to OrderResponse
        var response = _mapper.Map<OrderResponse>(order);

        return Result.Success(response);
    }
}

