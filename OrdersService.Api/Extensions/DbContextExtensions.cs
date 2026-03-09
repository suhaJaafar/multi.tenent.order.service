using OrdersService.Domain.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace OrdersService.Api.Extensions;

public static class DbContextExtensions
{
    public static void EnsureDbIsCreated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<OrdersContext>();
        context!.Database.Migrate();
        context.Database.CloseConnection();
    }
}

