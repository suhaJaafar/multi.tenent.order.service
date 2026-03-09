using System.Linq;
using System.Collections.Generic;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IdentityService.Api.Swagger{      
    /// <summary>
    /// Orders the Swagger "tags" (controllers) according to a custom list.
    /// Set your preferred order in DesiredTagOrder.
    /// Swagger UI is configured with tagsSorter = "none" so this order is respected in the UI.
    /// </summary>
    public class SwaggerTagOrderDocumentFilter : IDocumentFilter
    {
        // EDIT THIS to control the order shown in Swagger UI.
        // Any tag not listed here will appear after these, in alphabetical order.
        private static readonly string[] DesiredTagOrder = new[]
        {
            // Example custom order:
            // Put your most important controllers first
            "User",
            "Products",
            "Orders",
            "HealthCheck"
        };

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Collect all tags referenced by operations
            var discoveredTags = new Dictionary<string, OpenApiTag>();

            foreach (var path in swaggerDoc.Paths)
            {
                foreach (var operation in path.Value.Operations.Values)
                {
                    foreach (var tag in operation.Tags)
                    {
                        var key = tag.Name;
                        if (!discoveredTags.ContainsKey(key))
                        {
                            discoveredTags[key] = new OpenApiTag { Name = key, Description = tag.Description };
                        }
                    }
                }
            }

            // Merge with any existing explicit tags (preserving descriptions)
            if (swaggerDoc.Tags != null)
            {
                foreach (var tag in swaggerDoc.Tags)
                {
                    if (!discoveredTags.ContainsKey(tag.Name))
                    {
                        discoveredTags[tag.Name] = new OpenApiTag { Name = tag.Name, Description = tag.Description };
                    }
                    else if (!string.IsNullOrWhiteSpace(tag.Description))
                    {
                        discoveredTags[tag.Name].Description = tag.Description;
                    }
                }
            }

            // Create ordered list: first DesiredTagOrder, then the rest alphabetically
            var desiredOrderSet = new HashSet<string>(DesiredTagOrder);

            var ordered = new List<OpenApiTag>();

            // Add desired tags in the specified order if they exist
            foreach (var name in DesiredTagOrder)
            {
                if (discoveredTags.TryGetValue(name, out var t))
                {
                    ordered.Add(t);
                }
            }

            // Append remaining tags alphabetically
            var remaining = discoveredTags.Keys
                .Where(k => !desiredOrderSet.Contains(k))
                .OrderBy(k => k)
                .Select(k => discoveredTags[k]);

            ordered.AddRange(remaining);

            swaggerDoc.Tags = ordered;
        }
    }
}
