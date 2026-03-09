using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CatalogService.Api.Swagger;

/// <summary>
/// Orders the Swagger "tags" (controllers) according to a custom list.
/// </summary>
public class SwaggerTagOrderDocumentFilter : IDocumentFilter
{
    private static readonly string[] DesiredTagOrder = new[]
    {
        "Products",
        "HealthCheck"
    };

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var discoveredTags = new Dictionary<string, OpenApiTag>();

        foreach (var path in swaggerDoc.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                foreach (var tag in operation.Value.Tags)
                {
                    if (!discoveredTags.ContainsKey(tag.Name))
                    {
                        discoveredTags[tag.Name] = tag;
                    }
                }
            }
        }

        var orderedTags = new List<OpenApiTag>();
        foreach (var tagName in DesiredTagOrder)
        {
            if (discoveredTags.TryGetValue(tagName, out var tag))
            {
                orderedTags.Add(tag);
                discoveredTags.Remove(tagName);
            }
        }

        foreach (var remaining in discoveredTags.OrderBy(t => t.Key))
        {
            orderedTags.Add(remaining.Value);
        }

        swaggerDoc.Tags = orderedTags;
    }
}

