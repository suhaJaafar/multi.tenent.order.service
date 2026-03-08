using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Domain.Identity.DTOs;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiTenantOrderService.Api.Swagger;
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

        // Find and remove any ActiveUserData parameters from the Swagger documentation
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

