
using MultiTenantOrderService.Domain.DTOs;
using MultiTenantOrderService.Infrastructure.Utilities;

namespace MultiTenantOrderService.Infrastructure.Interfaces;
public interface IEnumService
{
    ServiceResponse<List<EnumToObject>> GetEnumValues(string enumName);
}

