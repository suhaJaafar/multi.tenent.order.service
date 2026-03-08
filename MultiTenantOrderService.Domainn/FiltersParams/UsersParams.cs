using MultiTenantOrderService.Domain.FiltersParams;
using MultiTenantOrderService.Domain.Enums;

namespace MultiTenantOrderService.Domain.FiltersParams;
public class UsersParams : BaseParams
{
    public UserType? UserType { get; set; }
}