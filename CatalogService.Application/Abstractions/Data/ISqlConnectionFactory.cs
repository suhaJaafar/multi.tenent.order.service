using System.Data;

namespace CatalogService.Application.DeleteProduct;
public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}