using CatalogService.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Extensions;

public static class DbContextExtensions
{
    public static void EnsureDbIsCreated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<CatalogContext>();
        context!.Database.Migrate();
        context.Database.CloseConnection();
    }
}

