using IdentityService.Domain.Abstractions;
using MediatR;
using IdentityService.Domain.Abstractions;

namespace IdentityService.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}