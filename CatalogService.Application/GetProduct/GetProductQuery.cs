using CatalogService.Application.Abstractions.Messaging;
namespace CatalogService.Application.GetProduct;

public record GetProductQuery(Guid ProductId) : IQuery<ProductResponse>;


