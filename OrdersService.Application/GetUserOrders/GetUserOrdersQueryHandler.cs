using AutoMapper;
using OrdersService.Application.Abstractions.Messaging;
using OrdersService.Application.CreateOrder;
using OrdersService.Domain.Abstractions;
using OrdersService.Domain.Order;

namespace OrdersService.Application.GetUserOrders;

internal sealed class GetUserOrdersQueryHandler : IQueryHandler<GetUserOrdersQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetUserOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<OrderResponse>>> Handle(
        GetUserOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var response = _mapper.Map<List<OrderResponse>>(orders);

        return Result.Success(response);
    }
}

