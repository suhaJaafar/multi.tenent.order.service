using MultiTenantOrderService.Domain.FiltersParams;
using MultiTenantOrderService.Domain.Enums;
using MultiTenantOrderService.Domain.Identity.Enums;

namespace MultiTenantOrderService.Domain.Identity.FiltersParams;
public class UsersParams : BaseParams
{
    public UserType? UserType { get; set; }
}