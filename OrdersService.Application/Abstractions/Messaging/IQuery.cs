using OrdersService.Domain.Abstractions;
using MediatR;

namespace OrdersService.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

