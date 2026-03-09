using CatalogService.Application.Abstractions.Messaging;
namespace CatalogService.Application.DeleteProduct;

public record DeleteProductCommand(Guid ProductId, Guid OwnerUserId) : ICommand;


