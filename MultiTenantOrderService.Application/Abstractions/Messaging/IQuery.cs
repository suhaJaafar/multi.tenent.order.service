using MultiTenantOrderService.Domain.Abstractions;
using MediatR;
using MultiTenantOrderService.Domain.Abstractions;

namespace MultiTenantOrderService.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}