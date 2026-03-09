
using IdentityService.Domain.DTOs;
using IdentityService.Infrastructure.Utilities;

namespace IdentityService.Infrastructure.Interfaces;
public interface IEnumService
{
    ServiceResponse<List<EnumToObject>> GetEnumValues(string enumName);
}

