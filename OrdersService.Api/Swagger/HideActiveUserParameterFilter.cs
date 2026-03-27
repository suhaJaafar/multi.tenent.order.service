using Microsoft.OpenApi.Models;
using OrdersService.Domain.Order.DTOs;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrdersService.Api.Swagger;

public class HideActiveUserParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parametersToRemove = operation.Parameters
            .Where(p => context.ApiDescription.ParameterDescriptions
                .Any(pd => pd.Name == p.Name && pd.Type == typeof(ActiveUserData)))
            .ToList();

        foreach (var parameter in parametersToRemove)
        {
            operation.Parameters.Remove(parameter);
        }
    }
}

