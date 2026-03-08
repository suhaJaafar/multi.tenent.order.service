using System.Data;

namespace MultiTenantOrderService.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}