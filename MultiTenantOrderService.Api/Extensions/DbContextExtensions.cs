using MultiTenantOrderService.Domain.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace MultiTenantOrderService.Api.Extensions
{
    public static class DbContextExtensions
    {
        public static void EnsureDbIsCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<OSContext>();
            context.Database.Migrate();
            context.Database.CloseConnection();
        }
    }
}
