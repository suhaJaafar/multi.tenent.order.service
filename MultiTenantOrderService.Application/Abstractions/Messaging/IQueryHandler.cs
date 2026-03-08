using MultiTenantOrderService.Domain.Abstractions;
using MediatR;
using MultiTenantOrderService.Application.Abstractions.Messaging;
using MultiTenantOrderService.Domain.Abstractions;

namespace MultiTenantOrderService.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}