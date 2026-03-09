using IdentityService.Domain.FiltersParams;
using IdentityService.Domain.Identity.Enums;
using IdentityService.Domain.Enums;

namespace IdentityService.Domain.Identity.FiltersParams;
public class UsersParams : BaseParams
{
    public UserType? UserType { get; set; }
}