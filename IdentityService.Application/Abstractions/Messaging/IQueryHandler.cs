using IdentityService.Domain.Abstractions;
using MediatR;
using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Domain.Abstractions;

namespace IdentityService.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}