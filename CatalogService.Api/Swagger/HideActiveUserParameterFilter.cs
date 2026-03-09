using Microsoft.OpenApi.Models;
using CatalogService.Domain.Product.DTOs;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CatalogService.Api.Swagger;

/// <summary>
/// Swagger operation filter to hide the ActiveUserData parameter from API documentation.
/// This parameter is automatically populated from JWT claims and should not be visible to API consumers.
/// </summary>
public class HideActiveUserParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            return;

        var activeUserParameters = operation.Parameters
            .Where(p => p.Name == "activeUser" ||
                       p.Schema?.Reference?.Id == nameof(ActiveUserData) ||
                       context.ApiDescription.ParameterDescriptions.Any(pd =>
                           pd.Name == p.Name && pd.Type == typeof(ActiveUserData)))
            .ToList();

        foreach (var parameter in activeUserParameters)
        {
            operation.Parameters.Remove(parameter);
        }
    }
}
