using CatalogService.Domain.Abstractions;
using MediatR;

namespace CatalogService.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

