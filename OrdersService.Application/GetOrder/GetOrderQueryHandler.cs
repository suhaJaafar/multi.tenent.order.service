using AutoMapper;
using OrdersService.Application.Abstractions.Messaging;
using OrdersService.Application.CreateOrder;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Order;

namespace OrdersService.Application.GetOrder;

internal sealed class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Result<OrderResponse>> Handle(
        GetOrderQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderResponse>(OrderErrors.NotFound);
        }

        var response = _mapper.Map<OrderResponse>(order);

        return Result.Success(response);
    }
}

